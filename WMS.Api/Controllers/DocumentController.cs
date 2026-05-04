using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos.WmsDocumentDtos;
using Services.Interfaces;

namespace WMS.Api.Controllers
{
    [ApiController]
    [Route("Documents")]
    public class DocumentController : ControllerBase
    {
        private readonly IWmsDocumentService _wmsDocumentService;
        private readonly IContractService _contractService;
        private readonly IPalletTypeService _palletTypeService;
        private readonly IMapper _mapper;
        private readonly ILogger<DocumentController> _logger;

        public DocumentController(IWmsDocumentService wmsDocumentService, IContractService contractService, IPalletTypeService palletTypeService, IMapper mapper , ILogger<DocumentController> logger)
        {
            _wmsDocumentService = wmsDocumentService;
            _palletTypeService = palletTypeService;
            _mapper = mapper;
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
        public async Task<ActionResult> CreateDocument(NewDocumentDto newDocument)
        {
            var requestedPalletIds = newDocument.items.Items.Keys.ToList();

            bool allValid = await _palletTypeService.AreAllPalletTypesValidAsync(requestedPalletIds);

            if (!allValid)
            {
                return BadRequest("One or more pallet types are invalid.");
            }

            var result = await _wmsDocumentService.AddDocument(newDocument.documentTypeId, newDocument.contractId, newDocument.items);

            return Ok(_mapper.Map<WmsDocumentInfoDto>(result));
        }

        [HttpPut("update/{documentId}")]
        public async Task<IActionResult> UpdateDocument([FromRoute]int documentId,NewDocumentItemsDto updatedDocumentItems)
        {
            var document = _wmsDocumentService.GetDocumentByIdAsync(documentId);

            if (document == null) {
                return BadRequest("No such document were found");
            }

            var result = await _wmsDocumentService.UpdateDocument(documentId, updatedDocumentItems);

            return Ok(result);
        }

    }
}
