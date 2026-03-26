using Domain.Entities;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class PalletTypeService
    {
        private readonly IRepository<PalletTypes> _palletTypeRepository;

        public PalletTypeService(IRepository<PalletTypes> palletTypeRepository)
        {
            _palletTypeRepository = palletTypeRepository;
        }

        public async Task<IEnumerable<PalletTypes>> GetAllPalletTypesAsync()
        {
            return await _palletTypeRepository.GetAllAsync();
        }

    }
}
