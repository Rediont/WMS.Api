using Microsoft.AspNetCore.Mvc;
using Services.Dtos;
using Services.Interfaces;

namespace WMS.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReceiptsController
    {
        private readonly IInboundReceiptService _inboundReceiptService;
        private readonly ILogger<ReceiptsController> _logger;

        public ReceiptsController(IInboundReceiptService inboundReceiptService, ILogger<ReceiptsController> logger)
        {
            _inboundReceiptService = inboundReceiptService;
            _logger = logger;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<InboundReceiptDto>>> GetAllReceipts()
        {
            var receipts = await _inboundReceiptService.GetAllReceiptsAsync();
            return new OkObjectResult(receipts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InboundReceiptDto>> GetReceiptById([FromRoute]int id)
        {
            var receipt = await _inboundReceiptService.GetReceiptByIdAsync(id);
            if (receipt == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(receipt);
        }

        [HttpGet("get-by-ids")]
        public async Task<ActionResult<List<InboundReceiptDto>>> GetReceiptsByIds([FromBody] List<int> ids)
        {
            var receipts = await _inboundReceiptService.GetReceiptsByIdsAsync(ids);
            return new OkObjectResult(receipts);
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateReceipt([FromForm]int contractId, [FromForm]int amount, [FromForm]int palletTypeId, [FromForm]List<int> palletIds)
        {
            var result = await _inboundReceiptService.AddReceipt(contractId, amount, palletTypeId, palletIds);
            if (!result)
            {
                return new BadRequestResult();
            }
            return new OkResult();
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteReceipt([FromQuery]int id)
        {
            var result = await _inboundReceiptService.DeleteReceiptByIdAsync(id);
            if (!result)
            {
                return new BadRequestResult();
            }
            return new OkResult();
        }
    }
}
