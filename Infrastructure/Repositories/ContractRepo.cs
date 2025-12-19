using Core.Entities;
using Infrastructure.DataBase;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    internal class ContractRepo : Repository<Contract>, IContractRepository
    {
        private readonly DbContext _dbContext;

        // можливо треба буде змінити статус контракту з "розірваний" на "призупинений"
        public void SuspendContract(int contractId)
        {
            Contract? targetContract = this._dbContext.Contracts.Find(contractId);
            if (targetContract == null)
            {
                throw new Exception("Contract not found");
            }
            targetContract.current_status = ContractStatus.Terminated;
            _dbContext.SaveChanges();
        }

        public void SetContractStatusToCompleted(int contractId)
        {
            Contract? targetContract = this._dbContext.Contracts.Find(contractId);
            if (targetContract == null)
            {
                throw new Exception("Contract not found");
            }
            targetContract.current_status = ContractStatus.Completed;
            _dbContext.SaveChanges();
        }

        public void SetContractStatusToInvalid(int contractId)
        {
            Contract? targetContract = this._dbContext.Contracts.Find(contractId);
            if (targetContract == null)
            {
                throw new Exception("Contract not found");
            }
            targetContract.current_status = ContractStatus.invalid;
            _dbContext.SaveChanges();
        }

        public List<Contract>? GetClientContracts(int clientId, ContractStatus? status = null)
        {
            var client = _dbContext.Clients.Find(clientId);
            if (client == null)
            {
                throw new Exception("Client not found");
            }
            var contracts = client.Contract_list.AsQueryable();
            if (status != null)
            {
                contracts = contracts.Where(c => c.current_status == status);
            }
            return contracts.ToList();
        }

        public void AddDeliveryToContract(int contractId, ContractShipment shipment)
        {
            var contract = _dbContext.Contracts.Find(contractId);
            if (contract == null)
            {
                throw new Exception("Contract not found");
            }
            contract.shipment_list.Add(shipment);
            _dbContext.SaveChanges();
        }

        public void AddShipmentToContract(int contractId, ContractDelivery dispatch)
        {
            var contract = _dbContext.Contracts.Find(contractId);
            if (contract == null)
            {
                throw new Exception("Contract not found");
            }
            contract.dispatch_list.Add(dispatch);
            _dbContext.SaveChanges();
        }

    }
}
