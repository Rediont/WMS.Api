using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Services.Dtos.ContractDtos;
using Services.Dtos.FilterDtos;
using Services.Dtos.LookUpDtos;
using Services.Interfaces;

namespace Services.Services
{
    public class ContractService : IContractService
    {
        private readonly IRepository<Contract> _contractRepository;
        private readonly IRepository<WmsDocument> _wmsDocumentRepository;
        private readonly IMapper _mapper;

        public ContractService(IRepository<Contract> contractRepository, IRepository<WmsDocument> wmsDocumentRepository, IMapper mapper)
        {
            _contractRepository = contractRepository;
            _wmsDocumentRepository = wmsDocumentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ContractDto>> GetAllContractsAsync(int? page)
        {
            int pageSize = 20;
            int pageIndex = page ?? 0;

            var contracts = await _contractRepository.Query()
                .OrderBy(c => c.Id)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync(); 

            return _mapper.Map<IEnumerable<ContractDto>>(contracts);
        }

        public async Task<IEnumerable<ContractDto>> GetAllContractsAsync(ContractFilterDto filter, int? page)
        {
            var query = _contractRepository.Query();

            if (filter.ClientIds != null && filter.ClientIds.Any())
            {
                query = query.Where(c => filter.ClientIds.Contains(c.ClientId));
            }

            if (filter.Status != null)
            {
                query = query.Where(c => c.CurrentStatus == filter.Status);
            }

            if (filter.DateFrom.HasValue)
                query = query.Where(c => c.StartDate >= filter.DateFrom.Value);

            if (filter.DateTo.HasValue)
                query = query.Where(c => c.StartDate <= filter.DateTo.Value);

            int pageIndex = page ?? 0;
            const int pageSize = 20;

            var contracts = await query
                .Include(c => c.Client)
                .OrderBy(c => c.Id)
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

        public async Task<IEnumerable<ContractInfoLookupDto>> LookupContractsInfo()
        {
            var contracts = await _contractRepository.Query().OrderBy(c => c.Id).Include(c => c.Client).Take(20).ToListAsync();
            return _mapper.Map<IEnumerable<ContractInfoLookupDto>>(contracts);
        }

        public async Task<int> LookupTotalPageCount()
        {
            return await _contractRepository.CountTotalPagesAsync();
        }

        public async Task<Contract> AddContractAsync(string name, DateTime startDate, DateTime endDate, ContractStatus status = ContractStatus.Active)
        {
            Contract newContract = new Contract
            {
                Name = name,
                StartDate = startDate,
                ExpirationDate = endDate,
                CurrentStatus = status,
                Documents = null,
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

        public async Task AddDocumentToContract(int id, WmsDocument document)
        {
            var contract = await _contractRepository.GetByIdAsync(id);
            if (contract == null) throw new Exception("Contract not found");
            if (contract.Documents == null)
            {
                contract.Documents = new List<WmsDocument>();
            }
            contract.Documents.Add(document);
            _contractRepository.Update(contract);
            await _contractRepository.SaveChangesAsync();
        }

        public async Task<ContractDetailsDto> GetContractDetailsAsync(int contractId)
        {
            var contractDetails = await _contractRepository.Query()
                .Include(c => c.Documents)
                .Where(c => c.Id == contractId)
                .Select(c => new ContractDetailsDto
                {
                    Id = c.Id,
                    ContractName = c.Name,
                    StartDate = c.StartDate,
                    ExpirationDate = c.ExpirationDate,
                    CurrentStatus = c.CurrentStatus,

                    ClientId = c.Client.Id,
                    ClientName = c.Client.Name,
                    ClientEmail = c.Client.Email,

                    Documents = _wmsDocumentRepository.Query()
                        .Where(d => d.ContractId == c.Id)
                        .Select(d => new RelatedDocumentDto
                        {
                            Id = d.Id,
                            DocumentType = d.DocumentType == DocumentType.InboundReceipt ? "Прибуття" : "Відвантаження",
                            CreationDate = d.CreationDate,
                            ExpectedTotalPallets = d.Items.Sum(i => i.ExpectedAmount)
                        })
                        .OrderByDescending(d => d.CreationDate) 
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (contractDetails == null)
            {
                throw new KeyNotFoundException($"Контракт з ID {contractId} не знайдено.");
            }

            return contractDetails;
        }


    }
}
