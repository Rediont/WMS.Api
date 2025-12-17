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
    internal class ContractRepo : IContactRepository
    {
        private readonly DbContext _dbContext;

        public List<Contract> GetAllContracts()
        {
            return _dbContext.Contracts.ToList();
        }

        public void AddContract(Contract contract)
        {
            if (_dbContext.Contracts.Contains(contract))
            {
                throw new Exception("Contract already exists");
            }
            this._dbContext.Contracts.Add(contract);
        }

        // можливо треба буде змінити статус контракту з "розірваний" на "призупинений"
        public void SuspendContract(string contractId)
        {
            Contract? targetContract = this._dbContext.Contracts.Find(contractId);
            if (targetContract == null)
            {
                throw new Exception("Contract not found");
            }
            targetContract.status = ContractStatus.Terminated;
            _dbContext.SaveChanges();
        }

        public void SetContractStatusToCompleted(string contractId)
        {
            Contract? targetContract = this._dbContext.Contracts.Find(contractId);
            if (targetContract == null)
            {
                throw new Exception("Contract not found");
            }
            targetContract.status = ContractStatus.Completed;
            _dbContext.SaveChanges();
        }

        public void SetContractStatusToInvalid(string contractId)
        {
            Contract? targetContract = this._dbContext.Contracts.Find(contractId);
            if (targetContract == null)
            {
                throw new Exception("Contract not found");
            }
            targetContract.status = ContractStatus.invalid;
            _dbContext.SaveChanges();
        }

        public Contract? GetContractById(string contractId, string clientId)
        {
            if (!_dbContext.Clients.Any(c => c.id == clientId))
            {
                throw new Exception("Client not found");
            }

            if (!_dbContext.Contracts.Any(c => c.id == contractId))
            {
                throw new Exception("Contract not found");
            }

            if (!_dbContext.Clients.Find(clientId)!.Contract_list.Any(c => c.id == contractId))
            {
                throw new Exception("Contract does not belong to the specified client");
            }

            if (clientId != null)
            {
                return _dbContext.Clients.Find(clientId)!.Contract_list.FirstOrDefault(c => c.id == contractId);
            }

            return this._dbContext.Contracts.Find(contractId);
        }

        public List<Contract>? GetClientContracts(string clientId, ContractStatus? status = null)
        {
            var client = _dbContext.Clients.Find(clientId);
            if (client == null)
            {
                throw new Exception("Client not found");
            }
            var contracts = client.Contract_list.AsQueryable();
            if (status != null)
            {
                contracts = contracts.Where(c => c.status == status);
            }
            return contracts.ToList();
        }


    }
}
