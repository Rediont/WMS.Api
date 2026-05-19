using Domain.Entities;
using Infrastructure.Interfaces;
using Services.Dtos.PalletBinding;
using Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Services.Services
{
    public class PalletBindingService : IPalletBindingService
    {
        private readonly IRepository<Pallet> _palletRepository;
        private readonly IRepository<Cell> _cellRepository;
        private readonly IRepository<WmsDocument> _wmsDocumentRepository;

        public PalletBindingService(IRepository<Pallet> palletRepository, IRepository<Cell> cellRepository, IRepository<WmsDocument> wmsDocumentRepository)
        {
            _palletRepository = palletRepository;
            _cellRepository = cellRepository;
            _wmsDocumentRepository = wmsDocumentRepository;
        }

        public async Task BindPalletToCell(int palletId, int cellId, int alleyIndex)
        {
            var pallet = await _palletRepository.GetByIdAsync(palletId);
            if (pallet == null) throw new Exception("Pallet not found");

            pallet.AlleyIndex = alleyIndex;
            pallet.CellIndex = cellId;
            pallet.PalletStatus = PalletStatus.Stored;
            _palletRepository.Update(pallet);

            var cell = await _cellRepository.Query()
                .FirstOrDefaultAsync(c => c.AlleyIndex == alleyIndex && c.CellIndex == cellId);

            if (cell != null)
            {
                cell.UsedCapacity = Math.Min(cell.TotalCapacity, cell.UsedCapacity + pallet.PalletType.RequiredCapacity);
                cell.IsOccupied = cell.UsedCapacity >= cell.TotalCapacity;
                _cellRepository.Update(cell);
            }

            await _palletRepository.SaveChangesAsync();
        }

        public async Task UnbindPalletFromCell(int palletId)
        {
            var pallet = await _palletRepository.GetByIdAsync(palletId);
            if (pallet == null) throw new Exception("Pallet not found");

            int? oldCellId = pallet.CellIndex;
            int? oldAlleyIndex = pallet.AlleyIndex;

            pallet.AlleyIndex = null;
            pallet.CellIndex = null;
            pallet.PalletStatus = PalletStatus.Arrived;
            _palletRepository.Update(pallet);

            if (oldCellId.HasValue && oldAlleyIndex.HasValue)
            {
                var cell = await _cellRepository.Query()
                    .FirstOrDefaultAsync(c => c.AlleyIndex == oldAlleyIndex.Value && c.CellIndex == oldCellId.Value);

                if (cell != null)
                {
                    cell.UsedCapacity = Math.Max(0, cell.UsedCapacity - pallet.PalletType.RequiredCapacity);
                    cell.IsOccupied = cell.UsedCapacity >= 2.8;
                    _cellRepository.Update(cell);
                }
            }

            await _palletRepository.SaveChangesAsync();
        }

        public async Task BindMultiple(Dictionary<int, int[]> cellPalletDict, int alleyIndex)
        {
            var allPalletIds = cellPalletDict.Values.SelectMany(ids => ids).ToList();
            var allCellsIds = cellPalletDict.Keys.ToList();

            var pallets = await _palletRepository.Query()
                .Where(p => allPalletIds.Contains(p.Id))
                .Include(p => p.PalletType)
                .ToListAsync();

            var cells = await _cellRepository.Query()
                .Where(c => c.AlleyIndex == alleyIndex && allCellsIds.Contains(c.CellIndex))
                .ToListAsync();

            foreach (var kvp in cellPalletDict)
            {
                int cellId = kvp.Key;
                int[] palletIds = kvp.Value;

                var cell = cells.FirstOrDefault(c => c.CellIndex == cellId);

                foreach (var palletId in palletIds)
                {
                    var pallet = pallets.FirstOrDefault(p => p.Id == palletId);
                    if (pallet != null)
                    {
                        pallet.AlleyIndex = alleyIndex;
                        pallet.CellIndex = cellId;
                        pallet.PalletStatus = PalletStatus.Stored;
                        _palletRepository.Update(pallet);

                        if (cell != null)
                        {
                            cell.UsedCapacity = Math.Min(cell.TotalCapacity, cell.UsedCapacity + pallet.PalletType.RequiredCapacity);
                            cell.IsOccupied = cell.UsedCapacity >= cell.TotalCapacity;
                        }
                    }
                }

                if (cell != null)
                {
                    _cellRepository.Update(cell);
                }
            }

            await _palletRepository.SaveChangesAsync();
        }


        public async Task<IEnumerable<ActivePalletsPerDocumentDto>> GetActivePalletsCountPerDocumentAsync()
        {
            var rawStats = await _wmsDocumentRepository.Query()
                    .Where(doc => doc.Pallets.Any(p =>
                        p.PalletStatus != PalletStatus.Shipped &&
                        p.PalletStatus != PalletStatus.Stored))
                    .OrderBy(doc => doc.CreationDate)
                    .Select(doc => new
                    {
                        DocumentId = doc.Id,
                        PalletGroups = doc.Pallets
                            .Where(p => p.PalletStatus != PalletStatus.Shipped && p.PalletStatus != PalletStatus.Stored)
                            .GroupBy(p => p.PalletTypeId)
                            .Select(g => new
                            {
                                PalletTypeId = g.Key,
                                Count = g.Count()
                            })
                            .ToList()
                    })
                    .ToListAsync();

            var stats = rawStats.Select(doc => new ActivePalletsPerDocumentDto
            {
                DocumentId = doc.DocumentId,
                ActivePalletsCount = doc.PalletGroups.ToDictionary(
                    g => g.PalletTypeId,
                    g => g.Count
                )
            });

            return stats;
        }

        public async Task<IEnumerable<PalletAssignmentDto>> GetUnboundPalletsForDocument(int documentId, int? palletTypeId)
        {
            var pallets = await this._palletRepository.Query()
                .Where(p =>
                    p.ArrivalDocumentId == documentId &&
                    p.PalletStatus != PalletStatus.Shipped &&
                    p.PalletStatus != PalletStatus.Stored &&
                    (!palletTypeId.HasValue || p.PalletTypeId == palletTypeId))
                .Select(p => new PalletAssignmentDto
                {
                    PalletId = p.Id,
                    PalletTypeId = p.PalletTypeId
                })
                .ToListAsync();

            return pallets;
        }

    }
}
