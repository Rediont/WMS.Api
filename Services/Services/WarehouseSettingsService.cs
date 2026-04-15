using Domain.Entities;
using Infrastructure.DataBase;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class WarehouseSettingsService : IWarehouseSettingsService
    {
        // Використовуємо твій Generic Repository
        private readonly IRepository<WarehouseSettings> _repository;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "WarehouseSettingsKey";

        public WarehouseSettingsService(IRepository<WarehouseSettings> repository, IMemoryCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<WarehouseSettings> GetSettingsAsync()
        {
            if (!_cache.TryGetValue(CacheKey, out WarehouseSettings settings))
            {
                // Шукаємо за Id = 1 через твій репозиторій
                settings = await _repository.GetByIdAsync(1);

                if (settings != null)
                {
                    _cache.Set(CacheKey, settings, TimeSpan.FromHours(24));
                }
            }
            return settings;
        }

        public async Task<WarehouseSettings> UpdateSettingsAsync(int numberOfAlleys, int numberOfFloorsPerAlley, int cellsPerAlleyFloor)
        {
            var settings = await _repository.GetByIdAsync(1);
            bool isNew = false;

            if (settings == null)
            {
                settings = new WarehouseSettings { Id = 1 };
                isNew = true;
            }

            // 1. Оновлюємо базові значення
            settings.NumberOfAlleys = numberOfAlleys;
            settings.NumberOfAlleyFloors = numberOfFloorsPerAlley;
            settings.NumberOfCellsInAlleyFloor = cellsPerAlleyFloor;

            // 2. Розраховуємо залежні значення (бізнес-логіка)
            settings.NumberOfCellsInAlley = numberOfFloorsPerAlley * cellsPerAlleyFloor;
            settings.NumberOfCells = numberOfAlleys * settings.NumberOfCellsInAlley;

            // 3. Зберігаємо через репозиторій
            if (isNew)
            {
                await _repository.AddAsync(settings);
            }
            else
            {
                _repository.Update(settings);
            }

            await _repository.SaveChangesAsync();

            // 4. Оновлюємо кеш, щоб усі інші сервіси одразу отримали нові розміри складу
            _cache.Set(CacheKey, settings, TimeSpan.FromHours(24));

            return settings;
        }
    }
}
