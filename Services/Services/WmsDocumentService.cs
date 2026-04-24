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
            if (document == null)
            {
                throw new KeyNotFoundException($"Document with ID {id} not found.");
            }
            return _mapper.Map<Task<WmsDocumentInfoDto>>(document);
        }

        public Task<List<WmsDocumentInfoDto>> GetDocumentsByIdsAsync(List<int> ids)
        {
            var documents = _wmsDocumentRepository.GetByIdsAsync(ids);
            return _mapper.Map<Task<List<WmsDocumentInfoDto>>>(documents);
        }

        public async Task<bool> AddInboundReceipt(int contractId, int amount, int palletType, List<int> palletIds)
        {
            var palletTypeEntity = await _palletTypeRepository.GetByIdAsync(palletType);
            var pallets = await _palletRepository.GetByIdsAsync(palletIds);

            if (palletTypeEntity == null)
            {
                throw new ArgumentException("Invalid pallet type ID.");
            }

            WmsDocument newReceipt = new WmsDocument
            {
                ContractId = contractId,
                CreationDate = DateTime.Now.Date,
                Pallets = pallets.ToList()
            };

            WmsDocumentItem newItem = new WmsDocumentItem
            {
                ExpectedAmount = amount,
                PalletTypeId = palletType
            };

            await _wmsDocumentItemRepository.AddAsync(newItem);
            await _wmsDocumentRepository.AddAsync(newReceipt);
            await _wmsDocumentItemRepository.SaveChangesAsync();
            await _wmsDocumentRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateInboundReceipt(int id, int contractId, int amount, int palletType, List<int> palletIds)
        {
            var receipt = await _wmsDocumentRepository.GetByIdAsync(id);
            if (receipt == null)
            {
                throw new KeyNotFoundException($"Receipt with ID {id} not found.");
            }

            var palletTypeEntity = await _palletTypeRepository.GetByIdAsync(palletType);
            var pallets = await _palletRepository.GetByIdsAsync(palletIds);

            if (palletTypeEntity == null)
            {
                throw new ArgumentException("Invalid pallet type ID.");
            }

            receipt.ContractId = contractId;
            receipt.Pallets = pallets.ToList();

            var item = await _wmsDocumentItemRepository.GetByIdAsync(receipt.Id);

            if (item != null)
            {
                item.ExpectedAmount = amount;
                item.PalletTypeId = palletType;
                _wmsDocumentItemRepository.Update(item);
                await _wmsDocumentItemRepository.SaveChangesAsync();
            }

            _wmsDocumentRepository.Update(receipt);
            await _wmsDocumentRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddOutboundShipment(int contractId, int amount, int palletType, List<int> palletIds)
        {
            var palletTypeEntity = await _palletTypeRepository.GetByIdAsync(palletType);
            var pallets = await _palletRepository.GetByIdsAsync(palletIds);

            if (palletTypeEntity == null)
            {
                throw new ArgumentException("Invalid pallet type ID.");
            }

            WmsDocument newShipment = new WmsDocument
            {
                ContractId = contractId,
                CreationDate = DateTime.Now.Date,
                Pallets = pallets.ToList()
            };

            WmsDocumentItem newItem = new WmsDocumentItem
            {
                ExpectedAmount = amount,
                PalletTypeId = palletType
            };

            await _wmsDocumentItemRepository.AddAsync(newItem);
            await _wmsDocumentRepository.AddAsync(newShipment);
            await _wmsDocumentItemRepository.SaveChangesAsync();
            await _wmsDocumentRepository.SaveChangesAsync();

            return true;

        }

        public async Task<bool> UpdateOutboundShipment(int id, int contractId, int amount, int palletType, List<int> palletIds)
        {
            var shipment = await _wmsDocumentRepository.GetByIdAsync(id);

            if (shipment == null)
            {
                throw new KeyNotFoundException($"Shipment with ID {id} not found.");
            }

            var palletTypeEntity = await _palletTypeRepository.GetByIdAsync(palletType);
            var pallets = await _palletRepository.GetByIdsAsync(palletIds);

            if (palletTypeEntity == null)
            {
                throw new ArgumentException("Invalid pallet type ID.");
            }

            shipment.ContractId = contractId;
            shipment.Pallets = pallets.ToList();

            var item = await _wmsDocumentItemRepository.GetByIdAsync(shipment.Id);
            if (item != null)
            {
                item.ExpectedAmount = amount;
                item.PalletTypeId = palletType;
                _wmsDocumentItemRepository.Update(item);
                await _wmsDocumentItemRepository.SaveChangesAsync();
            }

            _wmsDocumentRepository.Update(shipment);
            await _wmsDocumentRepository.SaveChangesAsync();

            return true;

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
