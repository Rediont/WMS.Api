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

        // додати обмеження у вигляді сторінок
        public async Task<ActionResult<List<OutboundShipmentDto>>> GetAllShipments()
        {
            var shipments = await _outboundShipmentService.GetAllShipmentsAsync();
            return new OkObjectResult(shipments);
        }

        public async Task<ActionResult<OutboundShipmentDto>> GetShipmentById(int id)
        {
            var shipment = await _outboundShipmentService.GetShipmentByIdAAsync(id);
            return new OkObjectResult(shipment);
        }

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

        public async Task<ActionResult> DeleteShipment(int id)
        {
            var result = await _outboundShipmentService.RemoveShipmentByIdAsync(id);
            return result ? new OkResult() : new BadRequestResult();
        }
    }
}
