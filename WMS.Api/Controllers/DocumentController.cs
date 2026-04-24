using Microsoft.AspNetCore.Mvc;
using Services.Dtos.WmsDocumentDtos;
using Services.Interfaces;

namespace WMS.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentController
    {
        private readonly IWmsDocumentService _wmsDocumentService;
        private readonly IContractService _contractService;
        private readonly ILogger<DocumentController> _logger;

        public DocumentController(IWmsDocumentService wmsDocumentService, IContractService contractService, ILogger<DocumentController> logger)
        {
            _wmsDocumentService = wmsDocumentService;
            _logger = logger;
            _contractService = contractService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<WmsDocumentInfoDto>>> GetAllDocuments([FromQuery]int? page)
        {
            var documents = await _wmsDocumentService.GetAllDocumentsAsync(page);
            return new OkObjectResult(documents);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WmsDocumentInfoDto>> GetDocumentById([FromRoute]int id)
        {
            var document = await _wmsDocumentService.GetDocumentByIdAsync(id);
            if (document == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(document);
        }

        [HttpGet("get-by-ids")]
        public async Task<ActionResult<List<WmsDocumentInfoDto>>> GetDocumentsByIds([FromBody] List<int> ids)
        {
            var documents = await _wmsDocumentService.GetDocumentsByIdsAsync(ids);
            return new OkObjectResult(documents);
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateDocument([FromForm]int contractId, [FromForm]int amount, [FromForm]int palletTypeId, [FromForm]List<int> palletIds)
        {
            var result = await _wmsDocumentService.AddInboundReceipt(contractId, amount, palletTypeId, palletIds);
            if (!result)
            {
                return new BadRequestResult();
            }
            return new OkResult();
        }
    }
}
