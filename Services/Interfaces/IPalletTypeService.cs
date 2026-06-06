using Domain.Entities;
using Services.Dtos.LookUpDtos;
using Services.Dtos.PalletDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IPalletTypeService
    {
        public Task<IEnumerable<PalletTypeLookupDto>> GetAllPalletTypesAsync();

        public Task<PalletTypeLookupDto> GetPalletTypeLookupByIdAsync(int id);

        public Task<PalletType> GetPalletTypeByIdAsync(int id);

        public Task<PalletType> AddPalletTypeAsync(PalletType palletType);

        public Task UpdatePalletTypeAsync(PalletType palletType);

        public Task DeletePalletTypeAsync(int id);

        public Task<bool> AreAllPalletTypesValidAsync(List<int> ids);
    }
}
