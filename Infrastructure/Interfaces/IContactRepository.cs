using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IContactRepository
    {
        public List<Contract> GetAllContracts();

        public void AddContract(Contract contract);

        public void SuspendContract(string contractId);

        public void SetContractStatusToCompleted(string contractId);

        public void SetContractStatusToInvalid(string contractId);

        public Contract? GetContractById(string contractId, string clientId);

        public List<Contract>? GetClientContracts(string clientId, ContractStatus? status = null);

    }
}
