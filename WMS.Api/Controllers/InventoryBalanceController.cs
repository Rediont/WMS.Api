using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos.WarehouseRemains;
using Services.Interfaces;

namespace WMS.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class InventoryBalanceController : ControllerBase
    {
        private readonly IInventoryBalanceService _inventoryBalanceService;
        private readonly ILogger<InventoryBalanceController> _logger;

        public InventoryBalanceController(IInventoryBalanceService inventoryBalanceService, ILogger<InventoryBalanceController> logger)
        {
            _inventoryBalanceService = inventoryBalanceService;
            _logger = logger;
        }

        [HttpPost("records/all")]
        public async Task<IActionResult> GetAllRecordsAsync(int page = 0)
        {
            var result = await this._inventoryBalanceService.GetAllInventoryBalanceRecordsAsync(page);
            return Ok(result);
        }

        [HttpPost("calculate/all")]
        public async Task<IActionResult> CalculateRemains([FromBody] CalculateRemainsRequestDto request)
        {
            try
            {
                var result = await this._inventoryBalanceService.CalculateRemainsAsync(request);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
