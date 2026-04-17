using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Services.Dtos.LookUpDtos;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class PalletTypeService : IPalletTypeService
    {
        private readonly IRepository<PalletTypes> _palletTypeRepository;
        private readonly IMapper _mapper;

        public PalletTypeService(IRepository<PalletTypes> palletTypeRepository, IMapper mapper)
        {
            _palletTypeRepository = palletTypeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PalletTypeLookupDto>> GetAllPalletTypesAsync()
        {
            return _mapper.Map<IEnumerable<PalletTypeLookupDto>>(await _palletTypeRepository.GetAllAsync());
        }

        public async Task<PalletTypeLookupDto> GetPalletTypeByIdAsync(int id)
        {
            var palletType = await _palletTypeRepository.GetByIdAsync(id);
            if (palletType == null)
            {
                throw new Exception("Pallet type not found");
            }
            return _mapper.Map<PalletTypeLookupDto>(palletType);
        }

        public async Task<PalletTypes> GetRealPalletTypeById(int id)
        {
            var palletType = await _palletTypeRepository.GetByIdAsync(id);
            if (palletType == null)
            {
                throw new Exception("Pallet type not found");
            }
            return palletType;
        }
    }
}
