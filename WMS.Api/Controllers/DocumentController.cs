using AutoMapper;
using Domain.Entities;
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
        public async Task<IActionResult> GetAllDocuments([FromQuery]int? page)
        {
            var documents = await _wmsDocumentService.GetAllDocumentsAsync(page);
            return new OkObjectResult(documents);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocumentById([FromRoute]int id)
        {
            var document = await _wmsDocumentService.GetDocumentByIdAsync(id);
            if (document == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(document);
        }

        [HttpGet("get-by-ids")]
        public async Task<IActionResult> GetDocumentsByIds([FromBody] List<int> ids)
        {
            var documents = await _wmsDocumentService.GetDocumentsByIdsAsync(ids);
            return new OkObjectResult(documents);
        }

        [HttpPost("create/receipt")]
        public async Task<IActionResult> CreateReceiptAsync(NewDocumentDto newDocument)
        {
            var requestedPalletIds = newDocument.Items.Items.Keys.ToList();

            bool allValid = await _palletTypeService.AreAllPalletTypesValidAsync(requestedPalletIds);

            if (!allValid)
            {
                return BadRequest("One or more pallet types are invalid.");
            }

            var result = await _wmsDocumentService.CreateReceiptAsync(newDocument.ContractId, newDocument.ClientId, newDocument.CreationDate, newDocument.Items);

            return Ok(_mapper.Map<WmsDocumentInfoDto>(result));
        }

        [HttpPost("create/shipment-by-pallets")]
        public async Task<IActionResult> CreateShipmentByPalletsAsync(NewShipmentDocumentDto newShipment)
        {
            try
            {
                var palletEntities = _mapper.Map<List<Pallet>>(newShipment.pallets);

                var shipmentResult = await this._wmsDocumentService.CreateShipmentAsync(newShipment.ClientId, newShipment.ContractId, newShipment.Date, palletEntities);
                return Ok(_mapper.Map<WmsDocumentInfoDto>(shipmentResult));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create/shipment")]
        public async Task<IActionResult> CreateShipmentAsync(NewDocumentDto newShipment)
        {
            try
            {
                var requestedPalletIds = newShipment.Items.Items.Keys.ToList();

                bool allValid = await _palletTypeService.AreAllPalletTypesValidAsync(requestedPalletIds);

                if (allValid)
                {
                    var shipmentResult = await this._wmsDocumentService.CreateShipmentAsync(newShipment.ClientId, newShipment.ContractId, newShipment.CreationDate, newShipment.Items);
                    return Ok(_mapper.Map<WmsDocumentInfoDto>(shipmentResult));
                }
                else
                {
                    throw new Exception("Not all types of pallets are valid");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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


        [HttpGet("weekly-stats")]
        public async Task<IActionResult> GetWeeklyStatsAsync()
        {
            var stats = await _wmsDocumentService.GetWeeklyStatsAsync();
            return Ok(stats);
        }
    }
}
