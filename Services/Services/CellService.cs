using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Services.Dtos;
using Services.Interfaces;
using System.Threading.Tasks;

namespace Services.Services
{
    internal class CellService : ICellService
    {
        private readonly IRepository<Cell> _cellRepository;
        private readonly IRepository<Pallet> _palletRepository;
        private readonly IMapper _mapper;   

        public CellService(IRepository<Cell> cellRepository, IRepository<Pallet> palletRepository , IMapper mapper)
        {
            _cellRepository = cellRepository;
            _palletRepository = palletRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CellDto>> GetAllCells()
        {
            var cells = await _cellRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CellDto>>(cells);
        }

        public async Task<CellDto?> GetCellById(int id)
        {
            var cell = await _cellRepository.GetByIdAsync(id);
            return cell is null ? null : _mapper.Map<CellDto>(cell);
        }

        public async Task<List<PalletInfoDto>> GetPalletsInCell(int cellId)
        {
            var cell = await _cellRepository.GetByIdAsync(cellId);
            var pallets = cell.StoredPallets;

            return _mapper.Map<List<PalletInfoDto>>(pallets);
        }

        public async Task<double> CalculateCellOccupancy(int cellId)
        {
            var cell = await _cellRepository.GetByIdAsync(cellId);
            if (cell is null || cell.totalCapacity == 0)
            {
                throw new ArgumentException("Cell not found or has zero capacity.");
            }
            double freeCapacity = cell.totalCapacity - cell.usedCapacity;
            return freeCapacity;
        }

        public async Task<bool> AddPalletToCell(int cellId, int palletId)
        {
            var cell = await _cellRepository.GetByIdAsync(cellId);
            var pallet = await _palletRepository.GetByIdAsync(palletId);

            if (cell is null || pallet is null)
            {
                return false;
            }

            //доробити логіку додавання палети до комірки 

        }
    }
}
