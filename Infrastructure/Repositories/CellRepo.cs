using Domain.Entities;
using Infrastructure.DataBase;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    internal class CellRepo : Repository<Cell>, ICellRepository
    {
        private readonly DbContext _dbContext;

        public void UpdateCellStatus(int cellId, CellStatus newStatus) {

            Cell? targetCell = this._dbContext.Cells.Find(cellId);

            if (targetCell == null)
            {
                throw new Exception("Cell not found");
            }

            targetCell.status = newStatus;
            _dbContext.SaveChanges();
        }

        public List<Cell> GetCellsByStatus(CellStatus status, int? alleyId = null) {

            if (!Enum.IsDefined(typeof(CellStatus), status)) {
                throw new Exception("Invalid Cell Status");
            }

            if (alleyId != null)
            {
                return _dbContext.Cells.Where(c => c.status == status && c.Alley_index == alleyId).ToList();
            }

            return _dbContext.Cells.Where(c => c.status == status).ToList();
        }
    }
}
