
using Core.Entities;
using Infrastructure.DataBase;

namespace Infrastructure.Repositories
{
    internal class ClientRepo
    {
        private readonly DbContext _dbContext;

        public List<Client> GetAllClients()
        {
            return _dbContext.Clients.ToList();
        }

        // add client to the database
        public void AddClient(Client client)
        {
            if (_dbContext.Clients.Contains(client))
            {
                throw new Exception("Client already exists");
            }

            this._dbContext.Clients.Add(client);
        }

        // remove client from the database
        public void RemoveClient(string clientId)
        {
            Client? targetClient = this._dbContext.Clients.Find(clientId);
            if (targetClient == null)
            {
                throw new Exception("Client not found");
            }
            _dbContext.Clients.Remove(targetClient);
            _dbContext.SaveChanges();
        }

        //add contract to a specific client
        public void AddContractToClient(string clientId, Contract contract)
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
        public void TerminateClientContract(string clientId, string contractId)
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
            targetContract.status = ContractStatus.Terminated;
            _dbContext.SaveChanges();
        }

        // get all contracts of a specific client
        public List<Contract> GetClientContracts(string clientId)
        {
            Client? client = GetAllClients().FirstOrDefault(c => c.id == clientId);

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
        public (string name, string phone) GetClientContacts(string clientId)
        {
            Client? client = GetAllClients().FirstOrDefault(c => c.id == clientId);
            if (client == null)
            {
                throw new Exception("Client not found");
            }
            return (client.contact_person_name, client.contact_phone);
        }


    }
}
