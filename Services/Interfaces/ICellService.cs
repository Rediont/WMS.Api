using Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICellService
    {
        public Task<IEnumerable<CellDto>> GetAllCells();

        public Task<CellDto?> GetCellById(int id);

        public Task<List<PalletInfoDto>> GetPalletsInCell(int cellId);

        public Task<double> CalculateCellOccupancy(int cellId);

        public Task<bool> AddPalletToCell(int cellId, int palletId);
    }
}
