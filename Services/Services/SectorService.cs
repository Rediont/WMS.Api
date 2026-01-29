using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Services.Dtos;
using Services.Interfaces;


namespace Services.Services
{
    internal class SectorService : ISectorService
    {
        private readonly IRepository<Sector> _sectorRepository;
        private readonly IMapper _mapper;

        public SectorService(IRepository<Sector> sectorRepository, IMapper mapper)
        {
            _sectorRepository = sectorRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SectorInfoDto>> GetAllSectorsAsync()
        {
            var sectors = await _sectorRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<SectorInfoDto>>(sectors);
        }

        public async Task<SectorInfoDto?> GetSectorByIdAsync(int id)
        {
            return _mapper.Map<SectorInfoDto?>(await _sectorRepository.GetByIdAsync(id));
        }

        public async Task<bool> IsSectorAvailableAsync(Sector newSector)
        {
            // Перевіряємо в базі, чи існує ХОЧ ОДИН конфліктний сектор
            bool hasConflict = await _sectorRepository.Query()
                .AnyAsync(existing =>
                    existing.AlleyIndex == newSector.AlleyIndex &&
                    existing.SectorIndex == newSector.SectorIndex &&
                    existing.FloorIndex == newSector.FloorIndex &&

                    (newSector.StartingCellIndex > existing.StartingCellIndex
                        ? newSector.StartingCellIndex : existing.StartingCellIndex)
                    <= (newSector.EndingCellIndex < existing.EndingCellIndex
                        ? newSector.EndingCellIndex : existing.EndingCellIndex) &&

                    newSector.ReserveStartDate < existing.ReserveEndDate &&
                    newSector.ReserveEndDate > existing.ReserveStartDate
                );

            // Якщо конфліктів НЕМАЄ (AnyAsync повернув false), то сектор доступний
            return !hasConflict;
        }

        public async Task AddSectorAsync(int alleyIndex, int startingCellId, int endingCellId, int floorIndex,int contractId , DateTime reserveEnd, DateTime? reserveStart = null)
        {
            Sector sector = new Sector
            {
                AlleyIndex = alleyIndex,
                FloorIndex = floorIndex,
                StartingCellIndex = startingCellId,
                EndingCellIndex = endingCellId,
                ContractId = contractId,
                ReserveStartDate = reserveStart ?? DateTime.Now,
                ReserveEndDate = reserveEnd
            };

            if (!await IsSectorAvailableAsync(sector))
            {
                throw new InvalidOperationException("Sector is not available for the specified time period.");
            }

            await _sectorRepository.AddAsync(sector);
            await _sectorRepository.SaveChangesAsync();
        }

        public async Task AddSectorAsync(SectorInfoDto sectorDto)
        {
            var sector = _mapper.Map<Sector>(sectorDto);

            if (!await IsSectorAvailableAsync(sector))
            {
                throw new InvalidOperationException("Sector is not available for the specified time period.");
            }

            await _sectorRepository.AddAsync(sector);
            await _sectorRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteSectorAsync(int id)
        {
            var sector = await _sectorRepository.GetByIdAsync(id);
            if (sector == null)
            {
                return false;
                throw new InvalidOperationException("Sector not found.");
            }
            _sectorRepository.Delete(sector);
            await _sectorRepository.SaveChangesAsync();
            return true;
        }

        public async Task UpdateSectorAsync(
            int id,
            int? startingCellIndex = null,
            int? endingCellIndex = null,
            DateTime? reserveEndDate = null)
        {

            // треба перевірку на макс. і мін. індекси в алеї 
            var sector = await _sectorRepository.GetByIdAsync(id);
            if (sector == null) throw new Exception("Contract not found");

            if (startingCellIndex.HasValue) sector.StartingCellIndex = startingCellIndex.Value;
            if (endingCellIndex.HasValue) sector.EndingCellIndex = endingCellIndex.Value;
            if (reserveEndDate.HasValue) sector.ReserveEndDate = reserveEndDate.Value;

            _sectorRepository.Update(sector);
            await _sectorRepository.SaveChangesAsync();
        }
    }
}
