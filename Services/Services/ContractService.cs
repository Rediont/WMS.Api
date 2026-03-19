using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Services.Dtos.ContractDtos;
using Services.Dtos.FilterDtos;
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

        public async Task<IEnumerable<ContractDto>> GetAllContractsAsync(int? page)
        {
            var contracts = await _contractRepository.GetAllAsync(page);
            return _mapper.Map<IEnumerable<ContractDto>>(contracts);
        }

        public async Task<IEnumerable<ContractDto>> GetAllContractsAsync(ContractFilterDto filter, int? page)
        {
            var query = _contractRepository.Query();

            // 1. Фільтрація за списком клієнтів (Виправлено)
            if (filter.ClientIds != null && filter.ClientIds.Any())
            {
                query = query.Where(c => filter.ClientIds.Contains(c.Id));
            }

            // 2. Фільтрація за статусом
            if (!string.IsNullOrEmpty(filter.Status.ToString()))
            {
                query = query.Where(c => c.CurrentStatus == filter.Status);
            }

            // 3. Фільтрація за датами
            if (filter.DateFrom.HasValue)
                query = query.Where(c => c.StartDate >= filter.DateFrom.Value);

            if (filter.DateTo.HasValue)
                query = query.Where(c => c.StartDate <= filter.DateTo.Value);

            // --- ПАГІНАЦІЯ ---
            int pageIndex = page ?? 0;
            const int pageSize = 20;

            var contracts = await query
                .OrderByDescending(c => c.StartDate) // Сортування обов'язкове для стабільної пагінації
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

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
