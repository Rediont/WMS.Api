using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.Dtos.WarehouseRemains;
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
        private readonly IRepository<InventoryBalance> _balanceRepository;
        private readonly IRepository<PalletType> _palletTypeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<InventoryBalance> _logger;

        public InventoryBalanceService(
            IRepository<WmsDocumentItem> itemRepository,
            IRepository<InventoryBalance> balanceRepository,
            IRepository<PalletType> palletTypeRepository,
            IMapper mapper,
            ILogger<InventoryBalance> logger)
        {
            _itemRepository = itemRepository;
            _balanceRepository = balanceRepository;
            _palletTypeRepository = palletTypeRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<WarehouseRemainsDto>> GetAllInventoryBalanceRecordsAsync(int page)
        {
            // Передаємо сторінку і перелічуємо всі потрібні зв'язки через кому!
            var result = await this._balanceRepository.GetAllAsync(page,
                b => b.Client,
                b => b.Contract,
                b => b.PalletType,
                b => b.Document,
                b => b.BatchDocument
            );

            return _mapper.Map<List<WarehouseRemainsDto>>(result);
        }

        public async Task<IEnumerable<WarehouseRemainsDto>> CalculateRemainsAsync(CalculateRemainsRequestDto request)
        {
            var query = _balanceRepository.Query().AsQueryable();

            if (request.StartDate.HasValue)
                query = query.Where(b => b.TransactionDate >= request.StartDate.Value);

            if (request.EndDate.HasValue)
                query = query.Where(b => b.TransactionDate <= request.EndDate.Value);

            var groupedData = await query
                .GroupBy(b => new
                {
                    // якщо групування не потрібне, ставимо 0, щоб EF об'єднав ці рядки
                    ClientId = request.GroupByClients ? b.ClientId : 0,
                    ContractId = request.GroupByContracts ? b.ContractId : 0,
                    PalletTypeId = request.GroupByPalletTypes ? b.PalletTypeId : 0,

                    ClientName = request.GroupByClients ? b.Client.Name : null,
                    ContractName = request.GroupByContracts ? b.Contract.Name : null,
                    PalletTypeName = request.GroupByPalletTypes ? b.PalletType.Name : null
                })
                .Select(g => new
                {
                    ClientId = g.Key.ClientId,
                    ClientName = g.Key.ClientName,
                    ContractId = g.Key.ContractId,
                    ContractName = g.Key.ContractName,
                    PalletTypeId = g.Key.PalletTypeId,
                    PalletTypeName = g.Key.PalletTypeName,
                    TotalAmount = g.Sum(x => x.Amount)
                })
                .Where(x => x.TotalAmount != 0)
                .ToListAsync();

            var result = new List<WarehouseRemainsDto>();

            foreach (var item in groupedData)
            {
                result.Add(new WarehouseRemainsDto
                {
                    // Якщо ID == 0, значить ми по ньому не групували, повертаємо null
                    ClientId = item.ClientId == 0 ? null : item.ClientId,
                    ClientName = item.ClientName,

                    ContractId = item.ContractId == 0 ? null : item.ContractId,
                    ContractName = item.ContractName,

                    PalletTypeId = item.PalletTypeId == 0 ? null : item.PalletTypeId,
                    PalletTypeName = item.PalletTypeName,
                    Amount = item.TotalAmount
                });
            }

            return result;
        }

        public async Task<int> CalculateSpecificRemainsAsync(int? clientId = null, int? contractId = null, int? palletTypeId = null)
        {
            var query = _itemRepository.Query();

            if (clientId.HasValue)
            {
                query = query.Where(i => i.Document.Contract.ClientId == clientId.Value);
            }

            if (contractId.HasValue)
            {
                query = query.Where(i => i.Document.ContractId == contractId.Value);
            }

            if (palletTypeId.HasValue)
            {
                query = query.Where(i => i.PalletTypeId == palletTypeId.Value);
            }

            return await query.SumAsync(i => i.ExpectedAmount);
        }

        public async Task<bool> AddInventoryBalanceRecord(int documentId, DateTime date, int clientId, int contractId, int palletTypeId, int amount, int batchDocumentId)
        {
            var newBalanceRecord = new InventoryBalance
            {
                DocumentId = documentId,
                ClientId = clientId,
                ContractId = contractId,
                TransactionDate = date,
                Amount = amount,
                BatchDocumentId = batchDocumentId
            };

            try
            {
                await this._balanceRepository.AddAsync(newBalanceRecord);
                await this._balanceRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) {
                _logger.LogError(message: "error ocured while atempting to add new inventory balance record" ,exception: ex);
                return false;
            }
        }

    }
}
