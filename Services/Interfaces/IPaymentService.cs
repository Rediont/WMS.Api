using Domain.Entities;
using Services.Dtos.PaymentDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IPaymentService
    {
        public Task<IEnumerable<Bill>> GetAllPaymentRecords(int page);

        public Task<BillDto> CalculateContractBillForClient(int clientId, int contractId, DateTime periodStart, DateTime periodEnd);

        public Task<bool> SaveBillAsync(BillDto bill);
    }
}
