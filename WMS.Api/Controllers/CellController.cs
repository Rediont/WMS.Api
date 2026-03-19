using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos;
using Services.Dtos.CellDtos;
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

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CellDto>))]
        public async Task<ActionResult<List<CellDto>>> GetAllCellsAsync()
        {
            var cells = await _cellService.GetAllCellsAsync();
            _logger.LogInformation("Retrieved {Count} cells", cells.Count());
            return new OkObjectResult(cells);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CellDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CellDto>> GetCellByIdAsync([FromRoute]int id)
        {
            var cell = await _cellService.GetCellByIdAsync(id);
            if (cell == null)
            {
                _logger.LogWarning("Cell with ID {Id} not found", id);
                return new NotFoundResult();
            }
            _logger.LogInformation("Retrieved cell with ID {Id}", id);
            return new OkObjectResult(cell);
        }

        [HttpGet("{cellId}/pallets")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PalletInfoDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<PalletInfoDto>>> GetPalletsInCell([FromRoute]int cellId)
        {
            if(await _cellService.GetCellByIdAsync(cellId) == null)
            {
                _logger.LogWarning("Cell with ID {CellId} not found", cellId);
                return new NotFoundResult();
            }
            var pallets = await _cellService.GetPalletsInCell(cellId);
            _logger.LogInformation("Retrieved {Count} pallets in cell ID {CellId}", pallets.Count, cellId);
            return new OkObjectResult(pallets);
        }

        [HttpGet("{cellId}/occupancy")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> CalculateCellOccupancy([FromRoute]int cellId)
        {
            try
            {
                var freeCapacity = await _cellService.CalculateCellOccupancy(cellId);
                _logger.LogInformation("Calculated free capacity for cell ID {CellId}: {FreeCapacity}", cellId, freeCapacity);
                return new OkObjectResult(freeCapacity);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Error calculating occupancy for cell ID {CellId}", cellId);
                return new NotFoundResult();
            }
        }

        [HttpPost("{cellId}/addPallet")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddPalletToCell([FromRoute] int cellId, [FromQuery]int palletId)
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
