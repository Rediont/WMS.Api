using Domain.Entities;
using Services.Dtos.WarehouseRemains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IInventoryBalanceService
    {
        public Task<IEnumerable<WarehouseRemainsDto>> GetAllInventoryBalanceRecordsAsync(int page);

        public Task<int> CalculateSpecificRemainsAsync(int? clientId = null, int? contractId = null, int? palletTypeId = null);

        public Task<bool> AddInventoryBalanceRecord(int documentId, DateTime date, int clientId, int contractId, int palletTypeId, int amount, int batchDocumentId);

        public Task<IEnumerable<WarehouseRemainsDto>> CalculateRemainsAsync(CalculateRemainsRequestDto request);
    }
}
