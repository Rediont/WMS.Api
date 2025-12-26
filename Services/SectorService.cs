using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    internal class SectorService
    {
        private readonly IRepository<Sector> _sectorRepository;
        public SectorService(IRepository<Sector> sectorRepository)
        {
            _sectorRepository = sectorRepository;
        }

        public async Task<IEnumerable<Sector>> GetAllSectorsAsync()
        {
            return await _sectorRepository.GetAllAsync();
        }

        public async Task<Sector?> GetSectorByIdAsync(int id)
        {
            return await _sectorRepository.GetByIdAsync(id);
        }

        public async Task<bool> IsSectorAvailableAsync(Sector newSector)
        {
            // Перевіряємо в базі, чи існує ХОЧ ОДИН конфліктний сектор
            bool hasConflict = await _sectorRepository.Query()
                .AnyAsync(existing =>
                    existing.alleyIndex == newSector.alleyIndex &&
                    existing.sectorIndex == newSector.sectorIndex &&
                    existing.floorIndex == newSector.floorIndex &&

                    (newSector.startingCellIndex > existing.startingCellIndex
                        ? newSector.startingCellIndex : existing.startingCellIndex)
                    <= (newSector.endingCellIndex < existing.endingCellIndex
                        ? newSector.endingCellIndex : existing.endingCellIndex) &&

                    newSector.reserveStartDate < existing.reserveEndDate &&
                    newSector.reserveEndDate > existing.reserveStartDate
                );

            // Якщо конфліктів НЕМАЄ (AnyAsync повернув false), то сектор доступний
            return !hasConflict;
        }

        public async Task AddSectorAsync(Sector sector)
        {
            if (!await IsSectorAvailableAsync(sector))
            {
                throw new InvalidOperationException("Sector is not available for the specified time period.");
            }

            await _sectorRepository.AddAsync(sector);
            await _sectorRepository.SaveChangesAsync();
        }

        public async Task DeleteSectorAsync(int id)
        {
            var sector = await _sectorRepository.GetByIdAsync(id);
            if (sector == null)
            {
                throw new InvalidOperationException("Sector not found.");
            }
            _sectorRepository.Delete(sector);
            await _sectorRepository.SaveChangesAsync();
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

            if (startingCellIndex.HasValue) sector.startingCellIndex = startingCellIndex.Value;
            if (endingCellIndex.HasValue) sector.endingCellIndex = endingCellIndex.Value;
            if (reserveEndDate.HasValue) sector.reserveEndDate = reserveEndDate.Value;

            _sectorRepository.Update(sector);
            await _sectorRepository.SaveChangesAsync();
        }
    }
}
