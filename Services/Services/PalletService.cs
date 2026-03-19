using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Services.Dtos;
using Services.Dtos.FilterDtos;
using Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Services.Services
{
    internal class PalletService : IPalletService
    {
        private readonly IRepository<Pallet> _palletRepository;
        private readonly IMapper _mapper;

        public PalletService(IRepository<Pallet> palletRepository, IMapper mapper)
        {
            _palletRepository = palletRepository;
            _mapper = mapper;
        }   

        public async Task<IEnumerable<PalletInfoDto>> GetAllPalletsAsync(int? page)
        {
            var pallets = await _palletRepository.GetAllAsync(page);
            return _mapper.Map<IEnumerable<PalletInfoDto>>(pallets);
        }

        public async Task<IEnumerable<PalletInfoDto>> GetAllPalletsAsync(PalletFilterDto filter, int? page)
        {
            var query = _palletRepository.Query();

            if (filter.ContractId.HasValue)
            {
                query = query.Where(p => p.InboundReceipt.ContractId == filter.ContractId.Value);
            }

            if (filter.PalletType.HasValue)
            {
                query = query.Where(p => p.PalletTypeId == filter.PalletType.Value);
            }

            int pageIndex = page ?? 0;
            const int pageSize = 20;

            var pallets = await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return _mapper.Map<IEnumerable<PalletInfoDto>>(pallets);
        }

        public Task<PalletInfoDto> GetPalletByIdAsync(int palletId)
        {
            var pallet = _palletRepository.GetByIdAsync(palletId);
            return _mapper.Map<Task<PalletInfoDto>>(pallet);
        }

        public Task<bool> AddPallet(int contractId, int palletType, int weight)
        {
            this._palletRepository.AddAsync(new Pallet
            {
                InboundReceiptId = contractId,
                PalletTypeId = palletType,
                weight = weight
            });
            return Task.FromResult(true);
        }

        // Додавання багатьох палет одного типу і одного товару для одного контракту
        public Task<bool> AddMultiplePallets(int amount, int contractId, int palletType, int weight)
        {
            throw new NotImplementedException();
        }

    }
}
