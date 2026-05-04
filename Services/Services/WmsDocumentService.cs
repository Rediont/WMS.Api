using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
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
        private readonly IRepository<WmsDocumentItem> _wmsDocumentItemRepository;
        private readonly IRepository<Pallet> _palletRepository;
        private readonly IRepository<PalletType> _palletTypeRepository;
        private readonly IMapper _mapper;

        public WmsDocumentService(IRepository<WmsDocument> wmsDocumentRepository, IRepository<WmsDocumentItem> wmsDocumentItemRepository, IRepository<Pallet> palletRepo, IRepository<PalletType> palletTypeRepo, IMapper mapper)
        {
            _wmsDocumentRepository = wmsDocumentRepository;
            _wmsDocumentItemRepository = wmsDocumentItemRepository;
            _palletRepository = palletRepo;
            _palletTypeRepository = palletTypeRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WmsDocumentInfoDto>> GetAllDocumentsAsync(int? page)
        {
            var documents = await _wmsDocumentRepository.GetAllAsync(page);
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

        public async Task<WmsDocument> AddDocument(int documentType ,int contractId, NewDocumentItemsDto newItems)
        {
            WmsDocument newDocument = new WmsDocument
            {
                DocumentType = (DocumentType)documentType,
                ContractId = contractId,
                CreationDate = DateTime.UtcNow
            };

            foreach (var item in newItems.Items)
            {
                var palletTypeId = item.Key;
                var amount = item.Value;

                newDocument.Items.Add( 
                    new WmsDocumentItem
                    {
                        ExpectedAmount = amount,
                        PalletTypeId = palletTypeId
                    });

                for (int i = 0; i < amount; i++)
                {
                    newDocument.Pallets.Add(new Pallet
                    {
                        PalletTypeId = palletTypeId
                    });
                }
            }

            await _wmsDocumentRepository.AddAsync(newDocument);
            await _wmsDocumentRepository.SaveChangesAsync();

            return newDocument;
        }

        public async Task<WmsDocument> UpdateDocument(int id, NewDocumentItemsDto newItems)
        {
            var document = await _wmsDocumentRepository.GetByIdAsync(
                id,
                d => d.Items,
                d => d.Pallets
                );

            // --- КРОК 1: ОНОВЛЕННЯ ТА ДОДАВАННЯ ---
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
            // Якщо користувач видалив рядок у формі (його немає в newItems), 
            // нам треба видалити цей WmsDocumentItem і всі його палети з бази
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
