using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Services.Dtos;
using Services.Interfaces;
using System.Threading.Tasks;

namespace Services.Services
{
    public class AlleyService : IAlleyService
    {
        private readonly IRepository<Alley> _alleyRepository;
        private readonly IRepository<Cell> _cellRepository;
        private readonly ISectorService _sectorService;
        private readonly IMapper _mapper;
        public AlleyService(IRepository<Alley> alleyRepository, ISectorService sectorService, IMapper mapper)
        {
            _alleyRepository = alleyRepository;
            _sectorService = sectorService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AlleyDto>> GetAllAlleysAsync()
        {
            var alleys = await _alleyRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<AlleyDto>>(alleys);
        }

        public async Task<AlleyDto> GetAlleyByIdAsync(int id)
        {
            Alley? alley = await _alleyRepository.GetByIdAsync(id);
            if(alley == null)
            {
                throw new Exception("Alley not found");
            }
            return _mapper.Map<AlleyDto>(alley);
        }

        public async void AddAlley(WarehouseSettings options)
        {

            Alley newAlley = new Alley
            {
                NumberOfFloors = options.NumberOfAlleyFloors,
                CellsPerFloor = options.NumberOfCellsInAlleyFloor,
                Sectors = null
            };

            await _alleyRepository.AddAsync(newAlley);
        }

        public async Task<List<int>> GetAlleysOccupancyRateAsync()
        {
            var alleyStats = await _cellRepository.Query()
                .GroupBy(c => c.AlleyIndex)
                .Select(g => new
                {
                    AlleyIndex = g.Key,
                    TotalCells = g.Count(),
                    OccupiedCells = g.Count(c => c.IsOccupied)
                })
                .OrderBy(x => x.AlleyIndex) 
                .ToListAsync();

            var percentages = alleyStats
                .Select(stat => stat.TotalCells == 0 ? 0 : (stat.OccupiedCells * 100) / stat.TotalCells)
                .ToList();

            return percentages;
        }

        // потенційно непотрібно
        private void DeleteAlley(int id)
        {
            Alley? alley = _alleyRepository.GetByIdAsync(id).Result;
            if (alley == null)
            {
                throw new Exception("Alley not found");
            }
            _alleyRepository.Delete(alley);
        }

        public void AddSectorToAlley(int alley_index, Sector sector)
        {
            Alley? targetAlley = _alleyRepository.GetByIdAsync(alley_index).Result;

            if (targetAlley == null)
            {
                throw new Exception("Alley not found");
            }
            
            if (sector.AlleyIndex != alley_index)
            {
                throw new Exception("Sector alley index does not match the target alley index");
            }

            if (targetAlley.Sectors == null)
            {
                targetAlley.Sectors = new List<Sector>();
            }

            targetAlley.Sectors.Add(sector);
            _alleyRepository.Update(targetAlley);
        }

        // видалити сектор з алеї за індексом сектору
        // потрібен тест про відсутність сектору
        public async Task RemoveSectorFromAlley(int alleyIndex, int sectorIndex)
        {
            Alley? targetAlley = await _alleyRepository.GetByIdAsync(alleyIndex, a => a.Sectors);

            if (targetAlley == null)
            {
                throw new Exception("Alley not found");
            }

            if (!targetAlley.Sectors.Any(s => s.SectorIndex == sectorIndex))
            {
                throw new Exception("Sector not found in the specified alley");
            }

            targetAlley.Sectors.Remove(targetAlley.Sectors.First(s => s.SectorIndex == sectorIndex));
            _alleyRepository.Update(targetAlley);
        }

    }
}
