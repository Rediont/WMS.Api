using Domain.Entities;
using Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IInboundReceiptService
    {
        public Task<IEnumerable<InboundReceiptDto>> GetAllReceiptsAsync();

        public Task<InboundReceiptDto> GetReceiptByIdAsync(int id);

        public Task<List<InboundReceiptDto>> GetReceiptsByIdsAsync(List<int> ids);

        public Task<bool> AddReceipt(int contractId, int amount, int palletType, List<int> palletIds);

        public Task<bool> DeleteReceiptByIdAsync(int id);
    }
}
