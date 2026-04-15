using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IWarehouseSettingsService
    {
        Task<WarehouseSettings> GetSettingsAsync();
        Task<WarehouseSettings> UpdateSettingsAsync(int numberOfAlleys, int numberOfFloorsPerAlley, int cellsPerAlleyFloor);
    }
}

