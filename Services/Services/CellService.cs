using Domain.Entities;
using Infrastructure.Interfaces;

namespace Services.Services
{
    internal class CellService
    {
        private readonly IRepository<Cell> _cellRepository;
        public CellService(IRepository<Cell> cellRepository)
        {
            _cellRepository = cellRepository;
        }

        public async Task<IEnumerable<Cell>> GetAllCells()
        {
            return await _cellRepository.GetAllAsync();
        }

        // Implementation details would go here
    }
}
