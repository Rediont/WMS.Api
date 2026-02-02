using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Services.Dtos;
using Services.Interfaces;

namespace Services.Services
{
    public class ClientService : IClientService
    {
        private readonly IRepository<Client> _clientRepository;
        private readonly IContractService _contractService;
        private readonly IMapper _mapper;

        public ClientService(IRepository<Client> clientRepository, IContractService contractService, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _contractService = contractService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClientInfoDto>> GetAllClients()
        {
            var clients = await _clientRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ClientInfoDto>>(clients);
        }

        public async Task<ClientInfoDto> GetClientByIdAsync(int id)
        {
            Client? client = await _clientRepository.GetByIdAsync(id);
            if (client == null)
            {
                throw new Exception("Client not found");
            }
            return _mapper.Map<ClientInfoDto>(client);
        }

        public async Task AddClient(string name, string clientEDRPO, string contactPersonName, string phoneNumber, string email)
        {
            Client newClient = new Client
            {
                Name = name,
                EDRPO = clientEDRPO,
                ContactPersonName = contactPersonName,
                ContactPersonPhone = phoneNumber,
                Email = email,
                ContractList = null
            };
            await _clientRepository.AddAsync(newClient);
        }

        public async Task UpdateClientAsync(
            int id,
            string? name = null,
            string? clientEDRPO = null,
            string? contactPersonName = null,
            string? phoneNumber = null,
            string? email = null)
        {
            // 1. Пошук клієнта
            var client = await _clientRepository.GetByIdAsync(id);
            if (client == null)
            {
                throw new Exception("Client not found");
            }

            // 2. Параметричне оновлення (замінюємо лише якщо передано значення)
            if (!string.IsNullOrWhiteSpace(name)) client.Name = name;
            if (!string.IsNullOrWhiteSpace(clientEDRPO)) client.EDRPO = clientEDRPO;
            if (!string.IsNullOrWhiteSpace(contactPersonName)) client.ContactPersonName = contactPersonName;
            if (!string.IsNullOrWhiteSpace(phoneNumber)) client.ContactPersonPhone = phoneNumber;
            if (!string.IsNullOrWhiteSpace(email)) client.Email = email;

            // 3. Збереження змін
            _clientRepository.Update(client);
            await _clientRepository.SaveChangesAsync();
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
            Contract? contract = client.ContractList.FirstOrDefault(c => c.Id == contractId);
            if (contract == null)
            {
                throw new Exception("Contract not found");
            }
            contract.CurrentStatus = status;

            _contractService.UpdateContractAsync(contract.Id, status: status).Wait();
            _clientRepository.Update(client);
        }

        public async Task<List<ContractDto>> GetClientContracts(int clientId)
        {
            var client = await _clientRepository.GetByIdAsync(clientId);

            if (client == null)
            {
                throw new KeyNotFoundException($"Client with ID {clientId} not found");
            }

            return _mapper.Map<List<ContractDto>>(client.ContractList);
        }

        public async Task<(string name, string phone)> GetCertainClientContacts(int clientId)
        {
            Client? client = await _clientRepository.GetByIdAsync(clientId);
            if (client == null)
            {
                throw new Exception("Client not found");
            }
            return (client.ContactPersonName, client.ContactPersonPhone);
        }
    }
}
