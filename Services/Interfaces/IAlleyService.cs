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
        public Task<IEnumerable<AlleyDto>> GetAllAlleys();

        public Task<AlleyDto> GetAlleyByIdAsync(int id);

        public void AddAlley(int height, int length, int width);

        public void AddSectorToAlley(int alleyIndex, Sector sector);

        public Task RemoveSectorFromAlley(int alleyIndex, int sectorIndex);
    }
}
