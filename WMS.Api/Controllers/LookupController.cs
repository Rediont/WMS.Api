using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos.LookupDtos;
using Services.Dtos.LookUpDtos;
using Services.Interfaces;
using Services.Services;

namespace WMS.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class LookupController
    {
        private readonly IPalletTypeService _palletTypeService;
        private readonly IClientService _clientService;
        private readonly IContractService _contractService;
        private readonly IWarehouseSettingsService _warehouseSettingsService;
        private readonly IWmsDocumentService _wmsDocumentService;
        private readonly IMapper _mapper;
        private readonly ILogger<LookupController> _logger;

        public LookupController(
            IPalletTypeService palletTypeService, 
            IClientService clientService, 
            IContractService contractService, 
            IWarehouseSettingsService warehouseSettingsService, 
            IWmsDocumentService wmsDocumentService, 
            IMapper mapper, 
            ILogger<LookupController> logger)
        {
            _palletTypeService = palletTypeService;
            _clientService = clientService;
            _contractService = contractService;
            _warehouseSettingsService = warehouseSettingsService;
            _wmsDocumentService = wmsDocumentService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll() // Змінили на async Task
        {
            try
            {
                // 1. Виконуємо запити ПО ЧЕРЗІ. 
                var clients = await _clientService.LookupClientsInfoAsync();
                var palletTypes = await _palletTypeService.GetAllPalletTypesAsync();
                var contracts = await _contractService.LookupContractsInfo();
                var warehouseSettings = await _warehouseSettingsService.GetWarehouseSettingsAsync();
                var documentTypes = await _wmsDocumentService.GetDocumentTypesAsync();

                // 2. Робимо мапінг, як ми це обговорювали раніше
                var mappedPalletTypes = palletTypes.Select(pt => new PalletTypeLookupDto
                {
                    Id = pt.Id,
                    Name = pt.Name
                }).ToList();

                // 3. Збираємо фінальний об'єкт
                var lookupData = new GlobalLookupDto
                {
                    Clients = clients,
                    Contracts = contracts,
                    PalletTypes = mappedPalletTypes,
                    WarehouseSettings = _mapper.Map<WarehouseSettingsLookupDto>(warehouseSettings),
                    DocumentTypes = documentTypes
                };

                _logger.LogInformation("Retrieved lookup data: {ClientCount} clients, {PalletTypeCount} pallet types, {ContractCount} contracts",
                    lookupData.Clients.Count(), lookupData.PalletTypes.Count(), lookupData.Contracts.Count());

                return new OkObjectResult(lookupData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving lookup data");
                return new StatusCodeResult(500); // StatusCode(500) теж зручніше
            }
        }

        [HttpGet("clients")]
        public async Task<IActionResult> GetClientsLookup()
        {
            try
            {
                var clients = await _clientService.LookupClientsInfoAsync();
                _logger.LogInformation("Retrieved {ClientCount} clients for lookup", clients.Count());
                return new OkObjectResult(clients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving clients for lookup");
                return new StatusCodeResult(500);
            }
        }

        [HttpGet("pallet-types")]
        public async Task<IActionResult> GetAllPalletTypes()
        {
            try
            {
                var palletTypes = await _palletTypeService.GetAllPalletTypesAsync();
                _logger.LogInformation("Retrieved {PalletTypeCount} pallet types", palletTypes.Count());
                return new OkObjectResult(palletTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pallet types");
                return new StatusCodeResult(500);
            }
        }

        [HttpGet("contracts")]
        public async Task<IActionResult> GetContractsLookup()
        {
            try
            {
                var contracts = await _contractService.LookupContractsInfo();
                _logger.LogInformation("Retrieved {ContractCount} contracts for lookup", contracts.Count());
                return new OkObjectResult(contracts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving contracts for lookup");
                return new StatusCodeResult(500);
            }
        }

        //public Task<IActionResult> GetClientTotalPages()
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<IActionResult> GetPalletTotalPages()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
