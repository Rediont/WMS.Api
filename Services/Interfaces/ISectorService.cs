using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ISectorService
    {
        public Task<IEnumerable<Sector>> GetAllSectorsAsync();

        public Task<Sector?> GetSectorByIdAsync(int id);

        public Task AddSectorAsync(int alleyIndex, int startingCellId, int endingCellId, int floorIndex, DateTime reserveEnd, DateTime? reserveStart = null);

        public Task DeleteSectorAsync(int id);

        public Task UpdateSectorAsync(
            int id,
            int? startingCellIndex = null,
            int? endingCellIndex = null,
            DateTime? reserveEndDate = null);

    }
}
