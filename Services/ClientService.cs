using Domain.Entities;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ClientService
    {
        private readonly IRepository<Client> _clientRepository;
        // можливо неправильно
        private readonly ContractService _contractService;
        public ClientService(IRepository<Client> clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<IEnumerable<Client>> GetAllClients()
        {
            return await _clientRepository.GetAllAsync();
        }

        public async Task<Client> GetClientByIdAsync(int id)
        {
            Client? client = await _clientRepository.GetByIdAsync(id);
            if (client == null)
            {
                throw new Exception("Client not found");
            }
            return client;
        }

        public async Task AddClient(string name, string clientEDRPO, string contactPersonName, string phoneNumber, string email)
        {
            Client newClient = new Client
            {
                name = name,
                EDRPO = clientEDRPO,
                contactPersonName = contactPersonName,
                contactPersonPhone = phoneNumber,
                email = email,
                ContractList = null
            };
            await _clientRepository.AddAsync(newClient);
        }

        public async Task UpdateClient(int id, string name, string clientEDRPO, string contactPersonName, string phoneNumber, string email)
        {
            Client? client = await _clientRepository.GetByIdAsync(id);
            if (client == null)
            {
                throw new Exception("Client not found");
            }
            client.name = name;
            client.EDRPO = clientEDRPO;
            client.contactPersonName = contactPersonName;
            client.contactPersonPhone = phoneNumber;
            client.email = email;
            _clientRepository.Update(client);
        }

        // потенційно непотрібно
        public async Task DeleteClient(int id)
        {
            Client? client = await _clientRepository.GetByIdAsync(id);
            if (client == null)
            {
                throw new Exception("Client not found");
            }
            _clientRepository.Delete(client);
        }

        public async Task AddContractToClient(int clientId, Contract contract)
        {
            Client? client = await _clientRepository.GetByIdAsync(clientId);
            if (client == null)
            {
                throw new Exception("Client not found");
            }
            if (client.ContractList == null)
            {
                client.ContractList = new List<Contract>();
            }
            client.ContractList.Add(contract);
            _clientRepository.Update(client);
        }

        public async Task SetClientContractStatus(int clientId, int contractId, ContractStatus status)
        {
            Client? client = await _clientRepository.GetByIdAsync(clientId);
            if (client == null)
            {
                throw new Exception("Client not found");
            }
            if (client.ContractList == null)
            {
                throw new Exception("Client has no contracts");
            }
            Contract? contract = client.ContractList.FirstOrDefault(c => c.id == contractId);
            if (contract == null)
            {
                throw new Exception("Contract not found");
            }
            contract.currentStatus = status;

            _contractService.UpdateContractAsync(contract.id, status: status).Wait();
            _clientRepository.Update(client);
        }

        public async Task<List<Contract>> GetClientContracts(int clientId)
        {
            Client? client = await _clientRepository.GetByIdAsync(clientId);
            if (client == null)
            {
                throw new Exception("Client not found");
            }
            return client.ContractList ?? new List<Contract>();
        }

        public async Task<(string name, string phone)> GetCertainClientContacts(int clientId)
        {
            Client? client = await _clientRepository.GetByIdAsync(clientId);
            if (client == null)
            {
                throw new Exception("Client not found");
            }
            return (client.contactPersonName, client.contactPersonPhone);
        }
    }
}
