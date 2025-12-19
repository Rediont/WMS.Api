using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IContractRepository : IRepository<Contract>
    {
        public void SuspendContract(int contractId);

        public void SetContractStatusToCompleted(int contractId);

        public void SetContractStatusToInvalid(int contractId);

        public List<Contract>? GetClientContracts(int clientId, ContractStatus? status = null);

        public void AddDeliveryToContract(int contractId, ContractShipment shipment);

        public void AddShipmentToContract(int contractId, ContractDelivery dispatch);

    }
}
