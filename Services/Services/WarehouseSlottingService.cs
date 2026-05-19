using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Services.Dtos.CellDtos;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class WarehouseSlottingService : IWarehouseSlottingService
    {
        private readonly IRepository<Cell> _cellRepository;
        private readonly IRepository<Alley> _alleyRepository;

        public WarehouseSlottingService(IRepository<Cell> cellRepository, IRepository<Alley> alleyRepository)
        {
            _cellRepository = cellRepository;
            _alleyRepository = alleyRepository;
        }

        public async Task<double[,]> BuildAlleyCapacityMatrixAsync(int alleyIndex)
        {
            var alley = await _alleyRepository.Query()
                            .FirstOrDefaultAsync(a => a.AlleyIndex == alleyIndex);

            if (alley == null) throw new ArgumentException("Alley not found");

            var cells = await _cellRepository.Query()
                .Where(c => c.AlleyIndex == alleyIndex)
                .ToListAsync();

            var matrix = new double[alley.NumberOfFloors, alley.CellsPerFloor];

            foreach (var cell in cells)
            {
                matrix[cell.FloorIndex, cell.CellIndex] = cell.TotalCapacity - cell.UsedCapacity;
            }

            return matrix;
        }

        public async Task<List<AvailableCellDto>> GetAvailableCellsAsyncInAlley(int alleyIndex, PalletType palletType)
        {
            var capacityMatrix = await BuildAlleyCapacityMatrixAsync(alleyIndex);
            var availableSpots = new List<AvailableCellDto>();

            int floors = capacityMatrix.GetLength(0);
            int cells = capacityMatrix.GetLength(1);

            for (int f = 0; f < floors; f++)
            {
                for (int c = 0; c < cells; c++)
                {
                    if (capacityMatrix[f, c] >= palletType.RequiredCapacity)
                    {
                        // Знайшли місце - додаємо в список
                        availableSpots.Add(new AvailableCellDto
                        {
                            FloorIndex = f,
                            CellId = c,
                            AvailableCapacity = capacityMatrix[f, c]
                        });
                    }
                }
            }

            return availableSpots;
        }

    }
}
