using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IClientService
    {
        public Task<IEnumerable<Client>> GetAllClients();

        public Task<Client> GetClientByIdAsync(int id);

        public Task AddClient(string name, string clientEDRPO, string contactPersonName, string phoneNumber, string email);

        public Task UpdateClientAsync(
            int id,
            string? name = null,
            string? clientEDRPO = null,
            string? contactPersonName = null,
            string? phoneNumber = null,
            string? email = null);

        public Task DeleteClient(int id);

        public Task AddContractToClient(int clientId, Contract contract);

        public Task SetClientContractStatus(int clientId, int contractId, ContractStatus status);

        public Task<List<Contract>> GetClientContracts(int clientId);

        public Task<(string name, string phone)> GetCertainClientContacts(int clientId);
    }
}
