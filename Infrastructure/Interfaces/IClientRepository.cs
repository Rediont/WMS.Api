using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IClientRepository : IRepository<Client>
    {
        //add contract to a specific client
        public void AddContractToClient(int clientId, Contract contract);

        // terminate a specific contract of a specific client
        public void TerminateClientContract(int clientId, int contractId);

        // get all contracts of a specific client
        public List<Contract> GetClientContracts(int clientId);

        // get contact person name and phone number of a specific client
        public (string name, string phone) GetCertainClientContacts(int clientId);

    }
}
