using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos.Alley;
using Services.Dtos.CellDtos;
using Services.Interfaces;

namespace WMS.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AlleyController : ControllerBase
    {
        private readonly ILogger<AlleyController> _logger;
        private readonly IAlleyService _alleyService;
        private readonly IPalletTypeService _palletTypeService;
        private readonly IMapper _mapper;
        private readonly IWarehouseSlottingService _warehouseSlottingService;

        public AlleyController(ILogger<AlleyController> logger, IAlleyService alleyService, IPalletTypeService palletTypeService, IMapper mapper, IWarehouseSlottingService warehouseSlottingService)
        {
            _logger = logger;
            _alleyService = alleyService;
            _palletTypeService = palletTypeService;
            _mapper = mapper;
            _warehouseSlottingService = warehouseSlottingService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAlleys()
        {
            var alleys = await _alleyService.GetAllAlleysAsync();
            _logger.LogInformation("Retrieved {AlleyCount} alleys", alleys.Count());
            return new OkObjectResult(alleys);
        }

        [HttpGet("{alleyId}")]
        public async Task<IActionResult> GetAlleyById([FromRoute] int alleyId)
        {
            var alley = await _alleyService.GetAlleyByIdAsync(alleyId);
            if (alley == null)
            {
                _logger.LogWarning("Alley with ID: {AlleyIndex} not found", alleyId);
                return new NotFoundResult();
            }
            _logger.LogInformation("Retrieved alley with ID: {AlleyIndex}", alleyId);
            return new OkObjectResult(alley);
        }

        [HttpGet("occupancy")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AlleyOccupancyDto>))]
        public async Task<IActionResult> GetAlleysOccupancyRate()
        {
            var occupancyRates = await _alleyService.GetAlleysOccupancyRateAsync();
            return new OkObjectResult(occupancyRates);
        }

        [HttpGet("{alleyId}/occupancy-map")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AlleyCellOccupancyMapDto>))]
        public async Task<IActionResult> GetAlleySlottingOccupancy([FromRoute] int alleyId)
        {
            var cellMap = await _alleyService.GetCellMapForAlleyAsync(alleyId);
            return new OkObjectResult(cellMap);
        }

        [HttpGet("{alleyId}/slotting/available-cells")]
        [ProducesResponseType(StatusCodes.Status200OK, Type= typeof(List<AvailableCellDto>))]
        public async Task<IActionResult> GetFreeCellsInAlley([FromRoute] int alleyId, [FromRoute] int palletTypeId)
        {
            var palletType = await _palletTypeService.GetPalletTypeByIdAsync(palletTypeId);
            if (palletType == null)
            {
                _logger.LogWarning("Pallet type with ID: {PalletTypeId} not found", palletTypeId);
                return new NotFoundResult();
            }
            var freeCells = await _warehouseSlottingService.GetAvailableCellsAsyncInAlley(alleyId, palletType);
            return new OkObjectResult(freeCells);
        }
    }
}
