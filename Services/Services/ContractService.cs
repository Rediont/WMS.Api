using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Services.Dtos;
using Services.Interfaces;

namespace Services.Services
{
    public class ContractService : IContractService
    {
        private readonly IRepository<Contract> _contractRepository;
        private readonly IMapper _mapper;

        public ContractService(IRepository<Contract> contractRepository, IMapper mapper)
        {
            _contractRepository = contractRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ContractDto>> GetAllContracts()
        {
            var contracts = await _contractRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ContractDto>>(contracts);
        }

        public async Task<ContractDto> GetContractByIdAsync(int id)
        {
            Contract? contract = await _contractRepository.GetByIdAsync(id);
            if (contract == null)
            {
                throw new Exception("Contract not found");
            }
            return _mapper.Map<ContractDto>(contract);
        }

        public async Task<Contract> AddContract(DateTime startDate, DateTime endDate, ContractStatus status = ContractStatus.Active)
        {
            // можливі зміни структури контракту
            Contract newContract = new Contract
            {
                StartDate = startDate,
                ExpirationDate = endDate,
                CurrentStatus = status,
                Inbounds = null,
                Outbounds = null
            };
            await _contractRepository.AddAsync(newContract);
            return newContract;
        }

        public async Task UpdateContractAsync(
            int id,
            DateTime? endDate = null,
            ContractStatus? status = null)
        {
            var contract = await _contractRepository.GetByIdAsync(id);
            if (contract == null) throw new Exception("Contract not found");

            if (endDate.HasValue) contract.ExpirationDate = endDate.Value;
            if (status.HasValue) contract.CurrentStatus = status.Value;

            _contractRepository.Update(contract);
            await _contractRepository.SaveChangesAsync();
        }

        public async Task AddInboundToContract(int id, InboundReceipt inbound)
        {
            var contract = await _contractRepository.GetByIdAsync(id);
            if (contract == null) throw new Exception("Contract not found");
            if (contract.Inbounds == null)
            {
                contract.Inbounds = new List<InboundReceipt>();
            }
            contract.Inbounds.Add(inbound);
            _contractRepository.Update(contract);
            await _contractRepository.SaveChangesAsync();
        }

        public async Task AddOutboundToContract(int id, OutboundShipment outbound)
        {
            var contract = await _contractRepository.GetByIdAsync(id);
            if (contract == null) throw new Exception("Contract not found");
            if (contract.Outbounds == null)
            {
                contract.Outbounds = new List<OutboundShipment>();
            }
            contract.Outbounds.Add(outbound);
            _contractRepository.Update(contract);
            await _contractRepository.SaveChangesAsync();
        }

    }
}
