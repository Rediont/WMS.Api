
using Domain.Entities;
using Infrastructure.DataBase;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    internal class ClientRepo : Repository<Client> ,IClientRepository
    {
        private readonly DbContext _dbContext;

        //add contract to a specific client
        public void AddContractToClient(int clientId, Contract contract)
        {
            Client? targetClient = this._dbContext.Clients.Find(clientId);
            if (targetClient == null)
            {
                throw new Exception("Client not found");
            }
            targetClient.Contract_list.Add(contract);
            _dbContext.SaveChanges();
        }

        // terminate a specific contract of a specific client
        public void TerminateClientContract(int clientId, int contractId)
        {
            Client? targetClient = this._dbContext.Clients.Find(clientId);
            if (targetClient == null)
            {
                throw new Exception("Client not found");
            }
            Contract? targetContract = targetClient.Contract_list.FirstOrDefault(c => c.id == contractId);
            if (targetContract == null)
            {
                throw new Exception("Contract not found");
            }
            targetContract.current_status = ContractStatus.Terminated;
            _dbContext.SaveChanges();
        }

        // get all contracts of a specific client
        public List<Contract> GetClientContracts(int clientId)
        {
            Client? client = GetAll().FirstOrDefault(c => c.id == clientId);

            if (client != null)
            {
                return client.Contract_list;
            }
            else
            {
                throw new Exception("Client not found");
            }
        }

        // get contact person name and phone number of a specific client
        public (string name, string phone) GetCertainClientContacts(int clientId)
        {
            Client? client = GetAll().FirstOrDefault(c => c.id == clientId);
            if (client == null)
            {
                throw new Exception("Client not found");
            }
            return (client.contact_person_name, client.contact_phone);
        }
    }
}
