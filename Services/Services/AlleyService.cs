using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Services.Dtos.Alley;
using Services.Dtos.CellDtos;
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
        public AlleyService(IRepository<Alley> alleyRepository, IRepository<Cell> cellRepository, ISectorService sectorService, IMapper mapper)
        {
            _alleyRepository = alleyRepository;
            _cellRepository = cellRepository;
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
            await _alleyRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<AlleyOccupancyDto>> GetAlleysOccupancyRateAsync()
        {
            var result = await _alleyRepository.Query()
                .Select(alley => new AlleyOccupancyDto
                {
                    AlleyId = alley.AlleyIndex,
                    // Якщо комірок немає або сума місткості 0, повертаємо 0
                    OccupancyPercentage = alley.Cells.Sum(c => c.TotalCapacity) > 0
                        ? Math.Round(alley.Cells.Sum(c => c.UsedCapacity) * 100 / alley.Cells.Sum(c => c.TotalCapacity), 2)
                        : 0
                })
                .OrderBy(dto => dto.AlleyId)
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<AlleyCellOccupancyMapDto>> GetCellMapForAlleyAsync(int alleyIndex)
        {
            var cells = await _cellRepository.Query()
                .Where(c => c.AlleyIndex == alleyIndex)
                .ToListAsync();

            var map = cells
                .GroupBy(c => c.FloorIndex)
                .Select(floorGroup => new AlleyCellOccupancyMapDto
                {
                    AlleyIndex = alleyIndex,
                    FloorIndex = floorGroup.Key,
                    CellOccupancies = floorGroup
                        .OrderBy(c => c.CellIndex)
                        .Select(c => new CellOccupancyDto
                        {
                            CellIndex = c.CellIndex,
                            FreeCapacity = Math.Max(0, c.TotalCapacity - c.UsedCapacity)
                        })
                        .ToList()
                })
                .OrderByDescending(f => f.FloorIndex)
                .ToList();

            return map;
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

        public async void AddSectorToAlley(int alley_index, Sector sector)
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
            await _alleyRepository.SaveChangesAsync();
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
            await _alleyRepository.SaveChangesAsync();
        }

    }
}
