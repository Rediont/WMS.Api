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

    }
}
