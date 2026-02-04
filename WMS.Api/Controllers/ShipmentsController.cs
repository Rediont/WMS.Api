using Microsoft.AspNetCore.Mvc;
using Services.Dtos;
using Services.Interfaces;

namespace WMS.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShipmentsController
    {
        private readonly IOutboundShipmentService _outboundShipmentService;
        private readonly ILogger<ShipmentsController> _logger;
        public ShipmentsController(IOutboundShipmentService outboundShipmentService, ILogger<ShipmentsController> logger)
        {
            _outboundShipmentService = outboundShipmentService;
            _logger = logger;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<OutboundShipmentDto>>> GetAllShipments()
        {
            var shipments = await _outboundShipmentService.GetAllShipmentsAsync();
            return new OkObjectResult(shipments);
        }


        // додати обмеження у вигляді сторінок
        [HttpGet("show")]
        public async Task<ActionResult<List<OutboundShipmentDto>>> GetShipmentsByIds([FromQuery]int pageId = 0)
        {
            throw new NotImplementedException();
        }

        [HttpGet("show/{id}")]
        public async Task<ActionResult<OutboundShipmentDto>> GetShipmentById([FromRoute]int id)
        {
            var shipment = await _outboundShipmentService.GetShipmentByIdAAsync(id);
            return new OkObjectResult(shipment);
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateShipment([FromBody] OutboundShipmentDto shipmentDto)
        {
            var result = await _outboundShipmentService.AddShipment(shipmentDto.ContractId, shipmentDto.ShippedPalletIds);
            if (result)
            {
                return new OkResult();
            }
            else
            {
                return new BadRequestResult();
            }
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteShipment([FromQuery]int id)
        {
            var result = await _outboundShipmentService.RemoveShipmentByIdAsync(id);
            return result ? new OkResult() : new BadRequestResult();
        }
    }
}
