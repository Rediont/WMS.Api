using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface ICellRepository : IRepository<Cell>
    {
        public void UpdateCellStatus(int cellId, CellStatus newStatus);

        public List<Cell> GetCellsByStatus(CellStatus status, int? alleyId = null);
    }
}
