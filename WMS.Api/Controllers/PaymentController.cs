using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos.PaymentDto;
using Services.Interfaces;

namespace WMS.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(
            IPaymentService paymentService,
            ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [HttpPost("calculate")]
        public async Task<IActionResult> Calculate([FromBody] BillRequestDto request)
        {
            _logger.LogInformation("Calculating bill with params: {Request}",
            System.Text.Json.JsonSerializer.Serialize(request));
            var result = await this._paymentService.CalculateContractBillForClient(request.ClientId, request.ContractId, request.PeriodStart, request.PeriodEnd);
            
            return Ok(result);
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] BillDto bill)
        {
            _logger.LogInformation("Saving bill with params: {Bill}",
            System.Text.Json.JsonSerializer.Serialize(bill));
            var result = await this._paymentService.SaveBillAsync(bill);

            if (result)
            {
                return Ok(result);
            }

            return BadRequest(); 
        }

    }
}
