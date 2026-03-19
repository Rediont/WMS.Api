using Services.Dtos;
using Services.Dtos.CellDtos;
using Services.Dtos.FilterDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IPalletService
    {
        public Task<IEnumerable<PalletInfoDto>> GetAllPalletsAsync(int? page);

        public Task<IEnumerable<PalletInfoDto>> GetAllPalletsAsync(PalletFilterDto palletFilter, int? page);

        public Task<PalletInfoDto?> GetPalletByIdAsync(int id);

        public Task<bool> AddPallet(int palletId, int palletType, int weight);

        public Task<bool> AddMultiplePallets(int amount, int contractId, int palletType, int weight);
    }
}
