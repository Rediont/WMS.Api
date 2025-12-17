using Core.Entities;
using Infrastructure.DataBase;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    internal class CellRepo : ICellRepository
    {
        private readonly DbContext _dbContext;

        public List<Cell> GetAllCells()
        {
            return _dbContext.Cells.ToList();
        }

        // потрібна перевірка для розмірів алеї
        public void AddCell(Cell cell)
        {
            if (_dbContext.Cells.Contains(cell))
            {
                throw new Exception("Cell already exists");
            }
            this._dbContext.Cells.Add(cell);
        }

        public void RemoveCell(string cellId)
        {
            Cell? targetCell = this._dbContext.Cells.Find(cellId);
            if (targetCell == null)
            {
                throw new Exception("Cell not found");
            }
            _dbContext.Cells.Remove(targetCell);
            _dbContext.SaveChanges();
        }

        public Cell GetCell(string cellId) {

            Cell? targetCell = this._dbContext.Cells.Find(cellId);

            if (targetCell == null)
            {
                throw new Exception("Cell not found");
            }
            return targetCell;
        }

        public void UpdateCellStatus(string cellId, CellStatus newStatus) {

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
