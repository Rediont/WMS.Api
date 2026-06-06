using Services.Dtos.CellDtos;
using Services.Dtos.PalletDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICellService
    {
        public Task<IEnumerable<CellDto>> GetAllCellsAsync(int? page);

        public Task<CellDto?> GetCellByIdAsync(int id);

        public Task<List<PalletInfoDto>> GetPalletsInCell(int cellId);

        public Task<double> CalculateCellOccupancy(int cellId);

        public Task<bool> AddPalletToCell(int cellId, int palletId);

        public Task<bool> RemovePalletFromCell(int cellId, int palletId);

        public Task<CellStatsDto> GetCellStatsAsync();
    }
}
