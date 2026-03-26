using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Dtos.FilterDtos;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Api.Controllers
{
    [ApiController]
    public class InventoryController
    {
        private readonly IPalletService _inventoryService;
        private readonly IPalletTypeService _palletTypeService;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(IPalletService inventoryService, IPalletTypeService palletTypeService, ILogger<InventoryController> logger)
        {
            _inventoryService = inventoryService;
            _palletTypeService = palletTypeService;
            _logger = logger;
        }

        [HttpGet("pallets")]
        public async Task<IActionResult> GetAllPallets([FromQuery] PalletFilterDto palletFilter, [FromQuery] int? page)
        {
            try
            {
                var pallets = await _inventoryService.GetAllPalletsAsync(palletFilter, page);
                _logger.LogInformation("Retrieved {PalletCount} pallets", pallets.Count());
                return new OkObjectResult(pallets);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pallets");
                return new StatusCodeResult(500);
            }
        }

        [HttpGet("pallet-types/all")]
        public async Task<IActionResult> GetAllPalletTypes()
        {
            try
            {
                var palletTypes = await _palletTypeService.GetAllPalletTypesAsync();
                _logger.LogInformation("Retrieved {PalletTypeCount} pallet types", palletTypes.Count());
                return new OkObjectResult(palletTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pallet types");
                return new StatusCodeResult(500);
            }
        }


        [HttpGet("pallets/{palletId}")]
        public async Task<IActionResult> GetPalletById([FromRoute] int palletId)
        {
            try
            {
                var pallet = await _inventoryService.GetPalletByIdAsync(palletId);
                if (pallet == null)
                {
                    _logger.LogWarning($"Pallet with ID: {palletId} not found");
                    return new NotFoundResult();
                }
                _logger.LogInformation($"Retrieved pallet with ID: {palletId}");
                return new OkObjectResult(pallet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving pallet with ID: {palletId}");
                return new StatusCodeResult(500);
            }
        }




    }
}
