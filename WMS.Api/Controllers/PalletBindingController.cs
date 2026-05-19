using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos.PalletBinding;
using Services.Interfaces;

namespace WMS.Api.Controllers
{
    [ApiController]
    [Route("pallet-binding")]
    public class PalletBindingController : ControllerBase
    {
        private readonly IPalletBindingService _palletBindingService;
        private readonly IPalletService _palletService;  
        private readonly IWmsDocumentService _wmsDocumentService;
        private readonly ILogger<PalletBindingController> _logger;
        private readonly IMapper _mapper;

        public PalletBindingController(IPalletBindingService palletBindingService, IPalletService palletService, IWmsDocumentService wmsDocumentService, ILogger<PalletBindingController> logger, IMapper mapper)
        {
            _palletBindingService = palletBindingService;
            _palletService = palletService;
            _wmsDocumentService = wmsDocumentService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("bind")]
        public async Task<IActionResult> BindPalletToCell(int palletId, int cellId, int alleyIndex)
        {
            var pallet = await _palletService.GetPalletByIdAsync(palletId);
            if (pallet == null)
            {
                return NotFound();
            }

            await _palletBindingService.BindPalletToCell(palletId, cellId, alleyIndex);
            return Ok();
        }

        [HttpPost("unbind")]
        public async Task<IActionResult> UnbindPalletFromCell(int palletId)
        {
            var pallet = await _palletService.GetPalletByIdAsync(palletId);
            if (pallet == null)
            {
                return NotFound();
            }

            await _palletBindingService.UnbindPalletFromCell(palletId);
            return Ok();
        }

        [HttpPost("bind-multiple")]
        public async Task<IActionResult> BindMultiple([FromBody] PalletBindingDictDto palletBindingDictDto)
        {
            await _palletBindingService.BindMultiple(palletBindingDictDto.CellPalletDict, palletBindingDictDto.AlleyIndex);
            return Ok();
        }

        [HttpGet("unbound-pallets/stats")]
        public async Task<IActionResult> GetUnboundPalletsStats()
        {
            var unboundPallets = await _palletBindingService.GetActivePalletsCountPerDocumentAsync();
            return Ok(unboundPallets);
        }

        [HttpGet("unbound-pallets/{documentId}")]
        public async Task<IActionResult> GetUnboundPalletsForDocument([FromRoute]int documentId, [FromQuery] int? palletTypeId)
        {
            var document = await this._wmsDocumentService.GetDocumentByIdAsync(documentId);
            if (document == null)
            {
                return NotFound();
            }
            var unboundPallets = await _palletBindingService.GetUnboundPalletsForDocument(documentId, palletTypeId);
            return Ok(unboundPallets);
        }
    }
}
