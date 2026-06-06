using Domain.Entities;
using Services.Dtos.CellDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IWarehouseSlottingService
    {
        public Task<double[,]> BuildAlleyCapacityMatrixAsync(int alleyIndex);
        public Task<List<AvailableCellDto>> GetAvailableCellsAsyncInAlley(int alleyId, PalletType palletType);

    }
}
