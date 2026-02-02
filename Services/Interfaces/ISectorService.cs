using Domain.Entities;
using Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ISectorService
    {
        public Task<IEnumerable<SectorInfoDto>> GetAllSectorsAsync();

        public Task<SectorInfoDto?> GetSectorByIdAsync(int id);

        public Task AddSectorAsync(int alleyIndex, int startingCellId, int endingCellId, int floorIndex, int contractId, DateTime reserveEnd, DateTime? reserveStart = null);

        public Task AddSectorAsync(SectorInfoDto sectorDto);

        public Task<bool> DeleteSectorAsync(int id);

        public Task UpdateSectorAsync(
            int id,
            int? startingCellIndex = null,
            int? endingCellIndex = null,
            DateTime? reserveEndDate = null);

    }
}
