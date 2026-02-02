using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos;
using Services.Interfaces;

namespace WMS.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SectorController
    {
        private readonly ILogger<SectorController> _logger;
        private readonly ISectorService _sectorService;
        private readonly IMapper _mapper;

        public SectorController(ILogger<SectorController> logger, ISectorService sectorService, IMapper mapper)
        {
            _logger = logger;
            _sectorService = sectorService;
            _mapper = mapper;
        }

        [HttpGet("get/all")]
        public async Task<IActionResult> GetAllSectors()
        {
            var sectors = await _sectorService.GetAllSectorsAsync();
            _logger.LogInformation("Retrieved {SectorCount} sectors", sectors.Count());
            return new OkObjectResult(sectors);
        }

        [HttpGet("get/{sectorId}")]
        public async Task<IActionResult> GetSectorById([FromRoute] int sectorId)
        {
            var sector = await _sectorService.GetSectorByIdAsync(sectorId);
            if (sector == null)
            {
                _logger.LogWarning("Sector with ID: {SectorId} not found", sectorId);
                return new NotFoundResult();
            }
            _logger.LogInformation("Retrieved sector with ID: {SectorId}", sectorId);
            return new OkObjectResult(sector);
        }

        // треба подумати над валідаторами для дто
        [HttpPost("create")]
        public async Task<IActionResult> CreateSector([FromBody] SectorInfoDto sectorDto)
        {
            await _sectorService.AddSectorAsync(sectorDto);
            _logger.LogInformation("Created new sector with ID: {SectorId}", sectorDto.SectorIndex);
            return new CreatedAtActionResult("GetSectorById", "Sector", new { sectorId = sectorDto.SectorIndex }, sectorDto);
        }

        [HttpDelete("delete/{sectorId}")]
        public async Task<IActionResult> DeleteSector([FromRoute] int sectorId)
        {
            var success = await _sectorService.DeleteSectorAsync(sectorId);
            if (!success)
            {
                _logger.LogWarning("Failed to remove sector with ID: {SectorId}", sectorId);
                return new NotFoundResult();
            }
            _logger.LogInformation("Removed sector with ID: {SectorId}", sectorId);
            return new NoContentResult();
        }
    }
}
