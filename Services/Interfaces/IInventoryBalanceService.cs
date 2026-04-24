using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IInventoryBalanceService
    {
        public Task<int> GetInventoryBalanceAsync();

        public Task<int> GetInventoryBalanceByContractAsync(int contractId);

        public Task<int> GetInventoryBalanceByClientAsync(int clientId);

        public Task<int> GetInventoryBalanceByPalletTypeAsync(int contractId, int palletTypeId);

    }
}
