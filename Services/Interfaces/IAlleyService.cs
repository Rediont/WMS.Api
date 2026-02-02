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
    internal interface IAlleyService
    {
        public Task<IEnumerable<AlleyDto>> GetAllAlleys();

        public Task<AlleyDto> GetAlleyByIdAAsync(int id);

        public void AddAlley(int height, int length, int width);

        public void AddSectorToAlley(int alley_index, Sector sector);

        public void RemoveSectorFromAlley(int alley_index, int sector_index);
    }
}
