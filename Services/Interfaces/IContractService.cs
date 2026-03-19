using Domain.Entities;
using Services.Dtos.ContractDtos;
using Services.Dtos.FilterDtos;
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

        public Task<Contract> AddContract(DateTime startDate, DateTime endDate, ContractStatus status = ContractStatus.Active);

        public Task UpdateContractAsync(int id, DateTime? endDate = null, ContractStatus? status = null);

        public Task AddInboundToContract(int id, InboundReceipt inbound);

        public Task AddOutboundToContract(int id, OutboundShipment outbound);
    }
}
