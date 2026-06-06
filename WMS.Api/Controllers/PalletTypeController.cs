using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos.PalletDtos;
using Services.Interfaces;

namespace WMS.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize()]
    public class PalletTypeController : ControllerBase
    {
        private readonly IPalletTypeService _palletTypeService;
        private readonly IMapper _mapper;
        private readonly ILogger<PalletTypeController> _logger;

        public PalletTypeController(IPalletTypeService palletTypeService, IMapper mapper, ILogger<PalletTypeController> logger)
        {
            _palletTypeService = palletTypeService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllPalletTypes()
        {
            var palletTypes = await _palletTypeService.GetAllPalletTypesAsync();
            return Ok(_mapper.Map<PalletTypeInfoDto>(palletTypes));
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPalletTypeById(int id)
        {
            try
            {
                var palletType = await _palletTypeService.GetPalletTypeByIdAsync(id);
                return Ok(_mapper.Map<PalletTypeInfoDto>(palletType));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting pallet type with id {id}");
                return NotFound(ex.Message);
            }
        }

        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddPalletType([FromBody] PalletTypeCreationDto palletTypeCreationDto)
        {
            try
            {
                var palletType = _mapper.Map<PalletType>(palletTypeCreationDto);
                PalletType newPallet = await _palletTypeService.AddPalletTypeAsync(palletType);
                return Ok(_mapper.Map<PalletTypeInfoDto>(newPallet));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding pallet type");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update/{palletId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePalletType(int palletId, [FromBody] PalletTypeUpdateDto palletTypeUpdateDto)
        {
            var id = palletTypeUpdateDto.Id;
            if(palletId != id)
            {
                return BadRequest("Pallet type ID mismatch");
            }
            var palletType = await _palletTypeService.GetPalletTypeByIdAsync(id);
            if (palletType == null)
            {
                return NotFound($"Pallet type with id {id} not found");
            }

            _mapper.Map<PalletTypeUpdateDto, PalletType>(palletTypeUpdateDto, palletType);

            await _palletTypeService.UpdatePalletTypeAsync(palletType);
            return Ok("Pallet type updated successfully");
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePalletType(int id)
        {
            try
            {
                await _palletTypeService.DeletePalletTypeAsync(id);
                return Ok("Pallet type deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting pallet type with id {id}");
                return BadRequest(ex.Message);
            }
        }


    }
}
