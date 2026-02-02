using Microsoft.AspNetCore.Mvc;
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

    }
}
