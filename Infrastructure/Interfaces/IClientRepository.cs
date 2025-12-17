using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IClientRepository
    {
        public List<Client> GetAllClients();

        public Client? GetClientById(Guid client);

        // add client to the database
        public void AddClient(Client client);

        // remove client from the database
        public void RemoveClient(Guid clientId);

        //add contract to a specific client
        public void AddContractToClient(Guid clientId, Contract contract);

        // terminate a specific contract of a specific client
        public void TerminateClientContract(Guid clientId, string contractId);

        // get all contracts of a specific client
        public List<Contract> GetClientContracts(Guid clientId);

        // get contact person name and phone number of a specific client
        public (string name, string phone) GetClientContacts(Guid clientId);

    }
}
