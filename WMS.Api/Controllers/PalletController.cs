using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos.PalletDtos;
using Services.Interfaces;

namespace WMS.Api.Controllers
{
    [Controller]
    [Route("[controller]")]
    [Authorize]
    public class PalletController : ControllerBase
    {
        private readonly IPalletService _palletService;
        private readonly IMapper _mapper;

        public PalletController(
            IPalletService palletService,
            IMapper mapper)
        {
            _palletService = palletService;
            _mapper = mapper;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllPallets(int? page)
        {
            var pallets = await this._palletService.GetAllPalletsAsync(page);
            return Ok(this._mapper.Map<IEnumerable<PalletInfoDto>>(pallets));
        }
    }
}
