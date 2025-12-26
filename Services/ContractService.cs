using Domain.Entities;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ContractService
    {
        private readonly IRepository<Contract> _contractRepository;
        public ContractService(IRepository<Contract> contractRepository)
        {
            _contractRepository = contractRepository;
        }

        public async Task<IEnumerable<Contract>> GetAllContracts()
        {
            return await _contractRepository.GetAllAsync();
        }

        public async Task<Contract> GetContractByIdAsync(int id)
        {
            Contract? contract = await _contractRepository.GetByIdAsync(id);
            if (contract == null)
            {
                throw new Exception("Contract not found");
            }
            return contract;
        }

        public async Task AddContract(DateTime startDate, DateTime endDate, ContractStatus status = ContractStatus.Active)
        {
            // можливі зміни структури контракту
            Contract newContract = new Contract
            {
                startDate = startDate,
                expirationDate = endDate,
                currentStatus = status,
                Inbounds = null,
                Outbounds = null
            };
            await _contractRepository.AddAsync(newContract);
        }

        public async Task UpdateContractAsync(
            int id,
            DateTime? endDate = null,
            ContractStatus? status = null)
        {
            var contract = await _contractRepository.GetByIdAsync(id);
            if (contract == null) throw new Exception("Contract not found");

            if (endDate.HasValue) contract.expirationDate = endDate.Value;
            if (status.HasValue) contract.currentStatus = status.Value;

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
