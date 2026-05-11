using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.Dtos.LookupDtos;
using Services.Dtos.WmsDocumentDtos;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class WmsDocumentService : IWmsDocumentService
    {
        private readonly IRepository<WmsDocument> _wmsDocumentRepository;
        private readonly IRepository<InventoryBalance> _balanceRepository;
        private readonly IRepository<Pallet> _palletRepository;
        private readonly ILogger<WmsDocumentService> _logger;
        private readonly IMapper _mapper;

        public WmsDocumentService(
            IRepository<WmsDocument> wmsDocumentRepository,
            IRepository<InventoryBalance> balanceRepository,
            IRepository<Pallet> palletRepository,
            ILogger<WmsDocumentService> logger,
            IMapper mapper)
        {
            _wmsDocumentRepository = wmsDocumentRepository;
            _balanceRepository = balanceRepository;
            _palletRepository = palletRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WmsDocumentInfoDto>> GetAllDocumentsAsync(int? page)
        {
            var documents = await _wmsDocumentRepository.GetAllAsync(page,
                d => d.Contract,
                d => d.Contract.Client
            );
            return _mapper.Map<IEnumerable<WmsDocumentInfoDto>>(documents);
        }

        public Task<WmsDocumentInfoDto> GetDocumentByIdAsync(int id)
        {
            var document = _wmsDocumentRepository.GetByIdAsync(id);
            return _mapper.Map<Task<WmsDocumentInfoDto>>(document);
        }

        public Task<List<WmsDocumentInfoDto>> GetDocumentsByIdsAsync(List<int> ids)
        {
            var documents = _wmsDocumentRepository.GetByIdsAsync(ids);
            return _mapper.Map<Task<List<WmsDocumentInfoDto>>>(documents);
        }

        public async Task<WmsDocument> CreateReceiptAsync(int contractId, int clientId, DateTime date, NewDocumentItemsDto newItems)
        {
            var newDocument = new WmsDocument
            {
                DocumentType = DocumentType.InboundReceipt,
                ContractId = contractId,
                CreationDate = date
            };

            foreach (var item in newItems.Items)
            {
                newDocument.Items.Add(new WmsDocumentItem { PalletTypeId = item.Key, ExpectedAmount = item.Value });

                for (int i = 0; i < item.Value; i++)
                {
                    newDocument.Pallets.Add(new Pallet { PalletTypeId = item.Key });
                }
            }

            await _wmsDocumentRepository.AddAsync(newDocument);
            await _wmsDocumentRepository.SaveChangesAsync();

            foreach (var item in newItems.Items)
            {
                await _balanceRepository.AddAsync(new InventoryBalance
                {
                    DocumentId = newDocument.Id,
                    ClientId = clientId,
                    ContractId = contractId,
                    PalletTypeId = item.Key,
                    TransactionDate = newDocument.CreationDate,
                    Amount = item.Value,
                    BatchDocumentId = newDocument.Id
                });
            }
            await _balanceRepository.SaveChangesAsync();

            return newDocument;
        }

        // версія з відправлянням конкретних палет (відправляє вибрані палети)
        public async Task<WmsDocument> CreateShipmentAsync(int clientId, int contractId,DateTime date , List<Pallet> pallets)
        {
            var groupedPallets = pallets
                .GroupBy(p => p.PalletTypeId)
                .ToDictionary(g => g.Key, g => g.Count());

            var shipmentDocument = new WmsDocument
            {
                DocumentType = DocumentType.OutboundShipment,
                ContractId = contractId,
                CreationDate = date,
                Items = new List<WmsDocumentItem>(),
                Pallets = new List<Pallet>()
            };

            var plannedBalanceRecords = new List<InventoryBalance>();

            foreach (var item in groupedPallets)
            {
                int palletTypeId = item.Key;
                int amountRequired = item.Value;

                shipmentDocument.Items.Add(new WmsDocumentItem
                {
                    PalletTypeId = palletTypeId,
                    ExpectedAmount = amountRequired
                });

                //Шукаємо партії, де є залишки, і сортуємо від найстарішої
                var availableBatches = await _balanceRepository.Query()
                    .Where(b => b.ClientId == clientId
                             && b.ContractId == contractId
                             && b.PalletTypeId == palletTypeId)
                    .GroupBy(b => b.BatchDocumentId) // Групуємо по ID приходу (партії)
                    .Select(g => new
                    {
                        BatchDocumentId = g.Key,
                        RemainingAmount = g.Sum(x => x.Amount), // Вираховуємо залишок партії (прихід - попередні відправки)
                        ReceiptDate = g.Min(x => x.TransactionDate) // Беремо дату створення цієї партії
                    })
                    .Where(b => b.RemainingAmount > 0) // Відкидаємо пусті партії
                    .OrderBy(b => b.ReceiptDate) // Сортуємо: від найстарішого (FIFO)
                    .ToListAsync();

                // Перевіряємо, чи взагалі вистачає палет на складі
                int totalAvailable = availableBatches.Sum(b => b.RemainingAmount);
                if (totalAvailable < amountRequired)
                {
                    throw new InvalidOperationException($"Недостатньо палет типу {palletTypeId}. На складі є: {totalAvailable}, потрібно: {amountRequired}.");
                }

                int amountToFulfill = amountRequired;

                foreach (var batch in availableBatches)
                {
                    if (amountToFulfill <= 0) break; // Якщо зібрали потрібну кількість - зупиняємо цикл

                    // Беремо рівно стільки, скільки нам треба, АБО скільки залишилося в цій партії (що менше)
                    int amountToTake = Math.Min(batch.RemainingAmount, amountToFulfill);

                    // Створюємо запис балансу (але поки без DocumentId, бо він ще не згенерований)
                    plannedBalanceRecords.Add(new InventoryBalance
                    {
                        ClientId = clientId,
                        ContractId = contractId,
                        PalletTypeId = palletTypeId,
                        TransactionDate = shipmentDocument.CreationDate,
                        Amount = -amountToTake,
                        BatchDocumentId = batch.BatchDocumentId // Вказуємо, з якої партії ми це забрали
                    });

                    // Зменшуємо кількість, яку ще залишилося знайти
                    amountToFulfill -= amountToTake;
                }
            }

            // зберігаємо сам документ, щоб БД згенерувала йому ID
            await _wmsDocumentRepository.AddAsync(shipmentDocument);
            await _wmsDocumentRepository.SaveChangesAsync();

            // Проставляємо згенерований DocumentId нашим записам у баланс і зберігаємо їх
            foreach (var record in plannedBalanceRecords)
            {
                record.DocumentId = shipmentDocument.Id; // Тепер ми знаємо ID
                await _balanceRepository.AddAsync(record);
            }

            await _balanceRepository.SaveChangesAsync();

            // зміна статусу палет на відправлені
            var palletIds = pallets.Select(p => p.Id).ToList();

            var physicalPallets = await _palletRepository.Query()
                .Where(p => palletIds.Contains(p.Id))
                .ToListAsync();

            var alreadyShippedPallets = physicalPallets
                .Where(p => p.PalletStatus == PalletStatus.Shipped)
                .ToList();

            if (alreadyShippedPallets.Any())
            {
                // Збираємо ID проблемних палет, щоб фронтенд/користувач знав, у чому біда
                var shippedIds = string.Join(", ", alreadyShippedPallets.Select(p => p.Id));
                throw new InvalidOperationException($"Неможливо створити відправлення. Наступні палети вже були відправлені раніше: {shippedIds}");
            }

            foreach (var pallet in physicalPallets)
            {
                pallet.PalletStatus = PalletStatus.Shipped;
                _palletRepository.Update(pallet);
            }

            await _palletRepository.SaveChangesAsync();

            return shipmentDocument;
        }

        // версія з відправлянням найстаріших палет (головне щоб кількість співпала)
        public async Task<WmsDocument> CreateShipmentAsync(int clientId, int contractId, DateTime date, NewDocumentItemsDto newItems)
        {
            var shipmentDocument = new WmsDocument
            {
                DocumentType = DocumentType.OutboundShipment,
                ContractId = contractId,
                CreationDate = date,
                Items = new List<WmsDocumentItem>(),
                Pallets = new List<Pallet>()
            };

            var plannedBalanceRecords = new List<InventoryBalance>();

            foreach (var item in newItems.Items)
            {
                int palletTypeId = item.Key;
                int amountRequired = item.Value;

                shipmentDocument.Items.Add(new WmsDocumentItem
                {
                    PalletTypeId = palletTypeId,
                    ExpectedAmount = amountRequired
                });

                // Шукаємо партії (приходи), де є залишки, і сортуємо від найстарішої (FIFO)
                var availableBatches = await _balanceRepository.Query()
                    .Where(b => b.ClientId == clientId
                             && b.ContractId == contractId
                             && b.PalletTypeId == palletTypeId)
                    .GroupBy(b => b.BatchDocumentId) // Групуємо по ID приходу (партії)
                    .Select(g => new
                    {
                        BatchDocumentId = g.Key,
                        RemainingAmount = g.Sum(x => x.Amount), // Залишок партії
                        ReceiptDate = g.Min(x => x.TransactionDate) // Дата партії
                    })
                    .Where(b => b.RemainingAmount > 0) // Відкидаємо пусті
                    .OrderBy(b => b.ReceiptDate)
                    .ToListAsync();

                int totalAvailable = availableBatches.Sum(b => b.RemainingAmount);
                if (totalAvailable < amountRequired)
                {
                    throw new InvalidOperationException($"Недостатньо палет типу {palletTypeId}. На складі є: {totalAvailable}, потрібно: {amountRequired}.");
                }

                int amountToFulfill = amountRequired;

                foreach (var batch in availableBatches)
                {
                    if (amountToFulfill <= 0) break;

                    int amountToTake = Math.Min(batch.RemainingAmount, amountToFulfill);

                    plannedBalanceRecords.Add(new InventoryBalance
                    {
                        ClientId = clientId,
                        ContractId = contractId,
                        PalletTypeId = palletTypeId,
                        TransactionDate = shipmentDocument.CreationDate,
                        Amount = -amountToTake, // Списуємо баланс
                        BatchDocumentId = batch.BatchDocumentId
                    });

                    var physicalPallets = await _palletRepository.Query()
                        .Where(p => p.ArrivalDocumentId == batch.BatchDocumentId // Палети саме з цього приходу
                                 && p.PalletTypeId == palletTypeId
                                 && p.PalletStatus != PalletStatus.Shipped)
                        .Take(amountToTake) // Беремо рівно стільки, скільки списуємо зараз
                        .ToListAsync();

                    foreach (var pallet in physicalPallets)
                    {
                        pallet.PalletStatus = PalletStatus.Shipped;
                        shipmentDocument.Pallets.Add(pallet);
                        this._palletRepository.Update(pallet);
                    }

                    amountToFulfill -= amountToTake;
                }
            }

            await _wmsDocumentRepository.AddAsync(shipmentDocument);
            await _wmsDocumentRepository.SaveChangesAsync();

            foreach (var record in plannedBalanceRecords)
            {
                record.DocumentId = shipmentDocument.Id;
                await _balanceRepository.AddAsync(record);
            }

            await _balanceRepository.SaveChangesAsync();
            await _palletRepository.SaveChangesAsync();

            return shipmentDocument;
        }


        public async Task<WmsDocument> UpdateDocument(int id, NewDocumentItemsDto newItems)
        {
            var document = await _wmsDocumentRepository.GetByIdAsync(
                id,
                d => d.Items,
                d => d.Pallets
                );

            foreach (var incomingItem in newItems.Items)
            {
                var palletTypeId = incomingItem.Key;
                var desiredAmount = incomingItem.Value;

                var existingItem = document.Items.FirstOrDefault(i => i.PalletTypeId == palletTypeId);

                if (existingItem != null)
                {
                    int currentAmount = existingItem.ExpectedAmount;
                    int difference = desiredAmount - currentAmount;

                    if (difference > 0)
                    {
                        // Треба ДОДАТИ нові (позитивна різниця)
                        for (int i = 0; i < difference; i++)
                        {
                            document.Pallets.Add(new Pallet { PalletTypeId = palletTypeId });
                        }
                    }
                    else if (difference < 0)
                    {
                        // Треба ВИДАЛИТИ зайві (негативна різниця)
                        int amountToRemove = Math.Abs(difference);

                        var palletsToRemove = document.Pallets
                            .Where(p => p.PalletTypeId == palletTypeId)
                            .Take(amountToRemove)
                            .ToList();

                        foreach (var p in palletsToRemove)
                        {
                            document.Pallets.Remove(p);
                        }
                    }

                    // Фіксуємо нову кількість у рядку документа
                    existingItem.ExpectedAmount = desiredAmount;
                }
                else
                {
                    // Такого типу палет ще не було — додаємо як новий рядок
                    document.Items.Add(new WmsDocumentItem
                    {
                        ExpectedAmount = desiredAmount,
                        PalletTypeId = palletTypeId
                    });

                    for (int i = 0; i < desiredAmount; i++)
                    {
                        document.Pallets.Add(new Pallet { PalletTypeId = palletTypeId });
                    }
                }
            }

            // --- КРОК 2: ПОВНЕ ВИДАЛЕННЯ ---
            // Якщо користувач видалив рядок у формі (його немає в newItems)
            var incomingPalletTypeIds = newItems.Items.Keys.ToList();
            var itemsToRemove = document.Items
                .Where(i => !incomingPalletTypeIds.Contains(i.PalletTypeId))
                .ToList();

            foreach (var itemToRemove in itemsToRemove)
            {
                // Видаляємо сам рядок
                document.Items.Remove(itemToRemove);

                // Видаляємо всі палети цього типу, що належали документу
                var palletsToRemove = document.Pallets
                    .Where(p => p.PalletTypeId == itemToRemove.PalletTypeId)
                    .ToList();

                foreach (var p in palletsToRemove)
                {
                    document.Pallets.Remove(p);
                }
            }

            _wmsDocumentRepository.Update(document);
            await _wmsDocumentRepository.SaveChangesAsync();

            return document;
        }

        public async Task<IEnumerable<DocumentTypeLookupDto>> GetDocumentTypesAsync()
        {
            var documentTypes = Enum.GetValues(typeof(DocumentType))
                                    .Cast<DocumentType>()
                                    .Select(dt => new DocumentTypeLookupDto
                                    {
                                        Id = (int)dt,           
                                        Name = dt.ToString()
                                    });

            return await Task.FromResult(documentTypes);
        }
    }
}
