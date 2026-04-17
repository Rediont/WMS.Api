using Domain.Entities;
using Infrastructure.DataBase;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
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
        private readonly IRepository<Alley> _alleyRepository;
        private readonly IRepository<Cell> _cellRepository;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "WarehouseSettingsKey";

        public WarehouseSettingsService(IRepository<WarehouseSettings> repository, IRepository<Alley> alleyRepository, IRepository<Cell> cellRepository, IMemoryCache cache)
        {
            _repository = repository;
            _alleyRepository = alleyRepository;
            _cellRepository = cellRepository;
            _cache = cache;
        }

        public async Task<WarehouseSettings> GetWarehouseSettingsAsync()
        {
            if (!_cache.TryGetValue(CacheKey, out WarehouseSettings settings))
            {
                // Шукаємо за Id = 1 через твій репозиторій
                settings = await _repository.Query().FirstOrDefaultAsync();

                if (settings != null)
                {
                    _cache.Set(CacheKey, settings, TimeSpan.FromHours(24));
                }
            }
            return settings;
        }

        public async Task<WarehouseSettings> UpdateWarehouseSettingsAsync(int numberOfAlleys, int numberOfFloorsPerAlley, int cellsPerAlleyFloor)
        {
            var settings = await _repository.Query().FirstOrDefaultAsync();
            bool isNew = false;

            if (settings == null)
            {
                settings = new WarehouseSettings
                {
                    Id = 1,
                    NumberOfAlleys = 0,
                    NumberOfAlleyFloors = 0,
                    NumberOfCellsInAlleyFloor = 0,
                    NumberOfCellsInAlley = 0,
                    NumberOfCells = 0
                };
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

            await SyncWarehouseLayoutAsync(settings);

            // 4. Оновлюємо кеш, щоб усі інші сервіси одразу отримали нові розміри складу
            _cache.Set(CacheKey, settings, TimeSpan.FromHours(24));

            return settings;
        }

        private async Task SyncWarehouseLayoutAsync(WarehouseSettings settings)
        {
            // Дістаємо всі існуючі алеї з БД
            var existingAlleys = await _alleyRepository.GetAllAsync();

            // Щоб не робити мільйон запитів до БД, дістаємо всі комірки одразу
            var existingCells = await _cellRepository.GetAllAsync();

            for (int a = 1; a <= settings.NumberOfAlleys; a++)
            {
                // 1. Перевіряємо, чи існує алея. Якщо ні - створюємо.
                var alley = existingAlleys.FirstOrDefault(x => x.AlleyIndex == a);
                if (alley == null)
                {
                    alley = new Alley
                    {
                        AlleyIndex = a,
                        NumberOfFloors = settings.NumberOfAlleyFloors,
                        CellsPerFloor = settings.NumberOfCellsInAlleyFloor
                    };
                    await _alleyRepository.AddAsync(alley);
                    await _alleyRepository.SaveChangesAsync(); // Зберігаємо одразу, щоб комірки могли до неї прив'язатись
                }

                // 2. Генеруємо комірки для цієї алеї
                int currentCellIndex = 1; // Наскрізна нумерація комірок в межах алеї

                for (int f = 0; f < settings.NumberOfAlleyFloors; f++)
                {
                    for (int c = 1; c <= settings.NumberOfCellsInAlleyFloor; c++)
                    {
                        // Перевіряємо, чи існує вже така комірка
                        bool cellExists = existingCells.Any(x => x.AlleyIndex == a && x.CellIndex == currentCellIndex);

                        if (!cellExists)
                        {
                            var newCell = new Cell
                            {
                                AlleyIndex = a,
                                FloorIndex = f,
                                CellIndex = currentCellIndex,
                                // totalCapacity, usedCapacity та isOccupied підтягнуться з дефолтних значень моделі
                            };
                            await _cellRepository.AddAsync(newCell);
                        }

                        currentCellIndex++;
                    }
                }
            }

            // Зберігаємо всі новостворені комірки одним махом
            await _cellRepository.SaveChangesAsync();
        }


    }
}
