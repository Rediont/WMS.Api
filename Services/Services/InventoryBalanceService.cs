using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class InventoryBalanceService : IInventoryBalanceService
    {
        private readonly IRepository<WmsDocumentItem> _itemRepository;
        private readonly IRepository<PalletType> _palletTypeRepository;

        public InventoryBalanceService(IRepository<WmsDocumentItem> itemRepository, IRepository<PalletType> palletTypeRepository)
        {
            _itemRepository = itemRepository;
            _palletTypeRepository = palletTypeRepository;
        }

        public async Task<int> GetInventoryBalanceAsync()
        {
            var items = await _itemRepository.GetAllAsync();
            return items.Sum(i => i.ExpectedAmount);
        }

        public async Task<int> GetInventoryBalanceByContractAsync(int contractId)
        {
            var items = await _itemRepository.Query().Where(i => i.Document.ContractId == contractId).ToListAsync();
            return items.Sum(i => i.ExpectedAmount);
        }

        public async Task<int> GetInventoryBalanceByClientAsync(int clientId)
        {
            var items = await _itemRepository.Query().Where(i => i.Document.Contract.ClientId == clientId).ToListAsync();
            return items.Sum(i => i.ExpectedAmount);
        }

        public async Task<int> GetInventoryBalanceByPalletTypeAsync(int contractId, int palletTypeId)
        {
            var items = await _itemRepository.Query().Where(i => i.Document.ContractId == contractId && i.PalletTypeId == palletTypeId).ToListAsync();
            return items.Sum(i => i.ExpectedAmount);

        }
    }
}


// доробити залишок при додаванні документа
