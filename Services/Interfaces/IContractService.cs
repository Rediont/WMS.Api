using Domain.Entities;
using Services.Dtos.ContractDtos;
using Services.Dtos.FilterDtos;
using Services.Dtos.LookUpDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IContractService
    {
        public Task<IEnumerable<ContractDto>> GetAllContractsAsync(int? page);
        public Task<IEnumerable<ContractDto>> GetAllContractsAsync(ContractFilterDto? filter, int? page);

        public Task<ContractDto> GetContractByIdAsync(int id);

        public Task<int> LookupTotalPageCount();

        public Task<IEnumerable<ContractInfoLookupDto>> LookupContractsInfo();

        public Task<Contract> AddContractAsync(string name, DateTime startDate, DateTime endDate, ContractStatus status = ContractStatus.Active);

        public Task UpdateContractAsync(int id, DateTime? endDate = null, ContractStatus? status = null);

        public Task AddDocumentToContract(int id, WmsDocument document);

    }
}
