using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
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
        private readonly IRepository<PalletType> _palletTypeRepository;
        private readonly IMapper _mapper;

        public PalletTypeService(IRepository<PalletType> palletTypeRepository, IMapper mapper)
        {
            _palletTypeRepository = palletTypeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PalletTypeLookupDto>> GetAllPalletTypesAsync()
        {
            return _mapper.Map<IEnumerable<PalletTypeLookupDto>>(await _palletTypeRepository.GetAllAsync());
        }

        public async Task<PalletTypeLookupDto> GetPalletTypeLookupByIdAsync(int id)
        {
            var palletType = await _palletTypeRepository.GetByIdAsync(id);
            if (palletType == null)
            {
                throw new Exception("Pallet type not found");
            }
            return _mapper.Map<PalletTypeLookupDto>(palletType);
        }

        public async Task<PalletType> GetPalletTypeByIdAsync(int id)
        {
            var palletType = await _palletTypeRepository.GetByIdAsync(id);
            if (palletType == null)
            {
                throw new Exception("Pallet type not found");
            }
            return palletType;
        }

        public async Task<PalletType> AddPalletTypeAsync(PalletType palletType)
        {
            await _palletTypeRepository.AddAsync(palletType);
            await _palletTypeRepository.SaveChangesAsync();
            return palletType;
        }

        public async Task UpdatePalletTypeAsync(PalletType palletType)
        {
            _palletTypeRepository.Update(palletType);
            await _palletTypeRepository.SaveChangesAsync();
        }

        public async Task DeletePalletTypeAsync(int id)
            {
                var palletType = await _palletTypeRepository.GetByIdAsync(id);
                if (palletType == null)
                {
                    throw new Exception("Pallet type not found");
                }
                _palletTypeRepository.Delete(palletType);
        }


        public async Task<bool> AreAllPalletTypesValidAsync(List<int> ids)
        {
            var existingCount = await _palletTypeRepository.Query()
                                              .Where(p => ids.Contains(p.Id))
                                              .CountAsync();

            return existingCount == ids.Count;
        }
    }
}
