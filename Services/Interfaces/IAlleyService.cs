using Domain.Entities;
using Infrastructure.Interfaces;
using Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IAlleyService
    {
        public Task<IEnumerable<AlleyDto>> GetAllAlleysAsync();

        public Task<AlleyDto> GetAlleyByIdAsync(int id);

        public Task<List<int>> GetAlleysOccupancyRateAsync();

        public void AddAlley(WarehouseSettings options);

        public void AddSectorToAlley(int alleyIndex, Sector sector);

        public Task RemoveSectorFromAlley(int alleyIndex, int sectorIndex);
    }
}
