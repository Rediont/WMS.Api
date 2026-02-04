using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Services.Dtos;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class InboundReceiptService : IInboundReceiptService
    {
        private readonly IRepository<InboundReceipt> _inboundReceiptRepository;
        private readonly IRepository<Pallet> _palletRepository;
        private readonly IRepository<PalletTypes> _palletTypeRepository;
        private readonly IMapper _mapper;

        public InboundReceiptService(IRepository<InboundReceipt> inboundReceiptRepository, IRepository<Pallet> palletRepo, IRepository<PalletTypes> palletTypeRepo, IMapper mapper)
        {
            _inboundReceiptRepository = inboundReceiptRepository;
            _palletRepository = palletRepo;
            _palletTypeRepository = palletTypeRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<InboundReceiptDto>> GetAllReceiptsAsync()
        {
            var receipts = await _inboundReceiptRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<InboundReceiptDto>>(receipts);
        }

        public Task<InboundReceiptDto> GetReceiptByIdAsync(int id)
        {
            var receipt =  _inboundReceiptRepository.GetByIdAsync(id);
            if (receipt == null)
            {
                throw new KeyNotFoundException($"Receipt with ID {id} not found.");
            }
            return _mapper.Map<Task<InboundReceiptDto>>(receipt);
        }

        public Task<List<InboundReceiptDto>> GetReceiptsByIdsAsync(List<int> ids)
        {
            var receipts = _inboundReceiptRepository.GetByIdsAsync(ids);
            return _mapper.Map<Task<List<InboundReceiptDto>>>(receipts);
        }

        public async Task<bool> AddReceipt(int contractId, int amount, int palletType, List<int> palletIds)
        {
            var palletTypeEntity = await _palletTypeRepository.GetByIdAsync(palletType);
            var pallets = await _palletRepository.GetByIdsAsync(palletIds);

            if (palletTypeEntity == null)
            {
                throw new ArgumentException("Invalid pallet type ID.");
            }

            InboundReceipt newReceipt = new InboundReceipt
            {
                ContractId = contractId,
                ReceiptDate = DateTime.Now,
                Amount = amount,
                PalletType = palletTypeEntity,
                Pallets = pallets.ToList()
            };
            _inboundReceiptRepository.AddAsync(newReceipt);
            return true;
        }

        public async Task<bool> DeleteReceiptByIdAsync(int id)
        {
            var receipt = await _inboundReceiptRepository.GetByIdAsync(id);
            if (receipt == null)
            {
                return false;
            }
            _inboundReceiptRepository.Delete(receipt);
            return true;
        }
    }
}
