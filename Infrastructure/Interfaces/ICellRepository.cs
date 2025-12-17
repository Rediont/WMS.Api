using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface ICellRepository
    {
        public List<Cell> GetAllCells();

        // потрібна перевірка для розмірів алеї
        public void AddCell(Cell cell);

        public void RemoveCell(string cellId);

        public Cell GetCell(string cellId);

        public void UpdateCellStatus(string cellId, CellStatus newStatus);

        public List<Cell> GetCellsByStatus(CellStatus status, int? alleyId = null);
    }
}
