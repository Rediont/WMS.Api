using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos.ContractDtos;
using Services.Dtos.FilterDtos;
using Services.Interfaces;
using System.Threading.Tasks;

namespace WMS.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Admin, Manager")]
    public class ContractController
    {
        private readonly IContractService _contractService;
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;
        private readonly ILogger<ContractController> _logger;

        public ContractController(
            IContractService contractService,
            IClientService clientService,
            IMapper mapper,
            ILogger<ContractController> logger)
        {
            _contractService = contractService;
            _clientService = clientService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllContracts([FromQuery] ContractFilterDto? filter, [FromQuery] int? page)
        {
            try
            {
                if (filter == null)
                {
                    var contracts = await _contractService.GetAllContractsAsync(page: page);
                    _logger.LogInformation("Retrieved {ContractCount} contracts without filters", contracts.Count());
                    return new OkObjectResult(contracts);
                }
                else
                {
                    var contracts = await _contractService.GetAllContractsAsync(filter, page);
                    _logger.LogInformation("Retrieved {ContractCount} contracts", contracts.Count());
                    return new OkObjectResult(contracts);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving contracts");
                return new StatusCodeResult(500);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContractById(int id)
        {
            var contract = await _contractService.GetContractByIdAsync(id);
            if (contract == null)
            {
                _logger.LogWarning("Contract with ID: {ContractId} not found", id);
                return new NotFoundResult();
            }
            _logger.LogInformation("Retrieved contract with ID: {ContractId}", id);
            return new OkObjectResult(contract);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddContract([FromBody] NewContractDataDto contractDataDto)
        {
            var client = await _clientService.GetClientByIdAsync(contractDataDto.ClientId);
            if (client == null)
            {
                _logger.LogWarning("Client with ID: {ClientId} not found", contractDataDto.ClientId);
                return new NotFoundResult();
            }

            ContractStatus status = ContractStatus.Inactive;

            if (contractDataDto.StartDate.Date == DateTime.Today)
            {
                status = ContractStatus.Active;
            }

            var contract = await _contractService.AddContractAsync(
                contractDataDto.Name,
                contractDataDto.StartDate,
                contractDataDto.EndDate,
                status
            );

            await _clientService.AddContractToClient(contractDataDto.ClientId, contract);

            _logger.LogInformation("Added contract with ID: {ContractId} to client with ID: {ClientId}", contract.Id, contractDataDto.ClientId);
            return new OkObjectResult(_mapper.Map<ContractDto>(contract));
        }

        [HttpPost("terminate")]
        public async Task<IActionResult> TerminateContract([FromQuery] int clientId, [FromQuery] int contractId)
        {
            var client = await _clientService.GetClientByIdAsync(clientId);
            if (client == null)
            {
                _logger.LogWarning("Client with ID: {ClientId} not found", clientId);
                return new NotFoundResult();
            }
            if (await _contractService.GetContractByIdAsync(contractId) == null)
            {
                _logger.LogWarning("Contract with ID: {ContractId} not found for client with ID: {ClientId}", contractId, clientId);
                return new NotFoundResult();
            }

            await _clientService.SetClientContractStatus(clientId, contractId, ContractStatus.Terminated);
            _logger.LogInformation("Terminated contract with ID: {ContractId} for client with ID: {ClientId}", contractId, clientId);
            return new OkResult();
        }

        [HttpPost("complete")]
        public async Task<IActionResult> CompleteContract([FromQuery] int clientId, [FromQuery] int contractId)
        {
            var client = await _clientService.GetClientByIdAsync(clientId);
            if (client == null)
            {
                _logger.LogWarning("Client with ID: {ClientId} not found", clientId);
                return new NotFoundResult();
            }
            if (await _contractService.GetContractByIdAsync(contractId) == null)
            {
                _logger.LogWarning("Contract with ID: {ContractId} not found for client with ID: {ClientId}", contractId, clientId);
                return new NotFoundResult();
            }

            await _clientService.SetClientContractStatus(clientId, contractId, ContractStatus.Completed);
            _logger.LogInformation("Completed contract with ID: {ContractId} for client with ID: {ClientId}", contractId, clientId);
            return new OkResult();
        }

        [HttpGet("total-pages")]
        public async Task<IActionResult> GetContractTotalPages()
        {
            return new OkObjectResult(await _contractService.LookupTotalPageCount());
        }

        //[HttpGet("info")]
        //public async Task<IActionResult> GetContractDetails([FromQuery] int id)
        //{
            
        //}
    }
}
