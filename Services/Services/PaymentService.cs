using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Services.Dtos.PaymentDto;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IRepository<Bill> _billRepository;
        private readonly IRepository<BillItem> _billItemRepository;
        private readonly IClientService _clientService;
        private readonly IContractService _contractService;
        private readonly IRepository<PalletType> _palletTypeRepository;
        private readonly IRepository<InventoryBalance> _inventoryBalanceRepository;

        public PaymentService(
            IRepository<Bill> billRepository,
            IRepository<BillItem> billItemRepository,
            IClientService clientService,
            IContractService contractService,
            IRepository<InventoryBalance> inventoryBalanceRepository,
            IRepository<PalletType> palletTypeRepository)
        {
            _billRepository = billRepository;
            _inventoryBalanceRepository = inventoryBalanceRepository;
            _palletTypeRepository = palletTypeRepository;
            _clientService = clientService;
            _contractService = contractService;
        }

        public async Task<IEnumerable<BillRecordDto>> GetAllPaymentRecords(int page)
        {

            var bills = await this._billRepository.GetAllAsync(page,
                b => b.Client,
                b => b.Contract);

            return bills.Select(b => new BillRecordDto
            {
                Id = b.Id,
                ClientId = b.ClientId,
                ClientName = b.Client?.Name ?? "N/A", // Переконайся, що навігаційна властивість підвантажена
                ContractId = b.ContractId,
                ContractName = b.Contract?.Name ?? "Невідомий контракт",
                CreationDate = b.PeriodStartDate,
                Total = (double)b.TotalCost,
                IsPaid = b.PaymentDate.HasValue
            });
        }

        public async Task<BillDto> CalculateContractBillForClient(int clientId, int contractId, DateTime periodStart, DateTime periodEnd)
        {
            var client = await this._clientService.GetClientByIdAsync(clientId);
            var contract = await this._contractService.GetContractByIdAsync(contractId);

            var initialBalances = await _inventoryBalanceRepository.Query()
                .Where(b => b.ClientId == clientId && b.ContractId == contractId && b.TransactionDate < periodStart)
                .GroupBy(b => b.PalletTypeId)
                .Select(g => new { PalletTypeId = g.Key, Total = g.Sum(x => x.Amount) })
                .ToDictionaryAsync(x => x.PalletTypeId, x => x.Total);

            var transactions = await _inventoryBalanceRepository.Query()
                .Where(b => b.ClientId == clientId &&
                            b.ContractId == contractId &&
                            b.TransactionDate >= periodStart &&
                            b.TransactionDate <= periodEnd)
                .Include(b => b.PalletType)
                .OrderBy(b => b.TransactionDate)
                .ToListAsync();

            var palletTypes = await _palletTypeRepository.GetAllAsync();

            var billItems = new List<BillItemDto>();

            foreach (var type in palletTypes)
            {
                int currentAmount = initialBalances.ContainsKey(type.Id) ? initialBalances[type.Id] : 0;
                long totalPalletDays = 0;

                DateTime cursor = periodStart.Date;
                DateTime end = periodEnd.Date;

                // Ітеруємо по днях
                while (cursor <= end)
                {
                    // Знаходимо зміни саме за цей день
                    var changesToday = transactions
                        .Where(t => t.PalletTypeId == type.Id && t.TransactionDate.Date == cursor)
                        .Sum(t => t.Amount);

                    currentAmount += changesToday;

                    totalPalletDays += currentAmount;

                    cursor = cursor.AddDays(1);
                }

                if (totalPalletDays > 0)
                {
                    billItems.Add(new BillItemDto
                    {
                        PalletTypeId = type.Id,
                        PalletTypeName = type.Name,
                        AmountOfDays = (int)totalPalletDays, // В даному контексті це "палето-дні"
                        CostPerDay = (decimal)type.Cost,
                        TotalCost = (decimal)totalPalletDays * type.Cost // Cost тут — це ціна за 1 палето-день
                    });
                }
            }

            return new BillDto
            {
                ClientId = clientId,
                ClientName = client.Name,
                ContractId = contractId,
                ContractName = contract.ContractName,
                Items = billItems,
                TotalCost = billItems.Sum(i => i.TotalCost),
                PeriodStart = periodStart,
                PeriodEnd = periodEnd
            };
        }

        public async Task<bool> SaveBillAsync(BillDto bill)
        {
            try
            {
                var billEntity = new Bill
                {
                    ClientId = bill.ClientId,
                    ContractId = bill.ContractId,
                    TotalCost = bill.TotalCost,
                    PeriodStartDate = bill.PeriodStart,
                    PeriodEndDate = bill.PeriodEnd,
                    BillItems = new List<BillItem>()
                };

                foreach (var item in bill.Items)
                {
                    billEntity.BillItems.Add(new BillItem
                    {
                        PalletTypeId = item.PalletTypeId,
                        AmountOfDays = item.AmountOfDays,
                        UnitPrice = item.CostPerDay,
                        TotalPrice = item.TotalCost,
                    });
                }

                await this._billRepository.AddAsync(billEntity);
                await this._billRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving bill: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                return false;
            }
        }
    }
}
