using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace WMS.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CellController
    {
        private readonly ILogger<CellController> _logger;
        private readonly ICellService _cellService;
        private readonly IMapper _mapper;

        public CellController(ILogger<CellController> logger, ICellService sectorService, IMapper mapper)
        {
            _logger = logger;
            _cellService = sectorService;
            _mapper = mapper;
        }

        public async Task<IActionResult> GetAllCellsAsync()
        {
            var cells = await _cellService.GetAllCellsAsync();
            _logger.LogInformation("Retrieved {Count} cells", cells.Count());
            return new JsonResult(cells);
        }

        public async Task<IActionResult> GetCellByIdAsync(int id)
        {
            var cell = await _cellService.GetCellByIdAsync(id);
            if (cell == null)
            {
                _logger.LogWarning("Cell with ID {Id} not found", id);
                return new NotFoundResult();
            }
            _logger.LogInformation("Retrieved cell with ID {Id}", id);
            return new JsonResult(cell);
        }

        public async Task<IActionResult> GetPalletsInCell(int cellId)
        {
            if(_cellService.GetCellByIdAsync(cellId) == null)
            {
                _logger.LogWarning("Cell with ID {CellId} not found", cellId);
                return new NotFoundResult();
            }
            var pallets = await _cellService.GetPalletsInCell(cellId);
            _logger.LogInformation("Retrieved {Count} pallets in cell ID {CellId}", pallets.Count, cellId);
            return new JsonResult(pallets);
        }

        public async Task<IActionResult> CalculateCellOccupancy(int cellId)
        {
            try
            {
                var freeCapacity = await _cellService.CalculateCellOccupancy(cellId);
                _logger.LogInformation("Calculated free capacity for cell ID {CellId}: {FreeCapacity}", cellId, freeCapacity);
                return new JsonResult(new { FreeCapacity = freeCapacity });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Error calculating occupancy for cell ID {CellId}", cellId);
                return new NotFoundResult();
            }
        }

        public async Task<IActionResult> AddPalletToCell(int cellId, int palletId)
        {
            var success = await _cellService.AddPalletToCell(cellId, palletId);
            if (!success)
            {
                _logger.LogWarning("Failed to add pallet ID {PalletId} to cell ID {CellId}", palletId, cellId);
                return new NotFoundResult();
            }
            _logger.LogInformation("Added pallet ID {PalletId} to cell ID {CellId}", palletId, cellId);
            return new OkResult();
        }
    }
}
