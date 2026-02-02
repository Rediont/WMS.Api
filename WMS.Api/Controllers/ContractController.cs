using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Threading.Tasks;

namespace WMS.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContractController
    {
        private readonly IContractService _contractService;
        private readonly IClientService _clientService;
        private readonly ILogger<ContractController> _logger;

        public ContractController(
            IContractService contractService,
            IClientService clientService,
            ILogger<ContractController> logger)
        {
            _contractService = contractService;
            _clientService = clientService;
            _logger = logger;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllContracts()
        {
            try
            {
                var contracts = await _contractService.GetAllContracts();
                _logger.LogInformation("Retrieved {ContractCount} contracts", contracts.Count());
                return new OkObjectResult(contracts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving contracts");
                return new StatusCodeResult(500);
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddContract([FromQuery]int clientId, [FromBody]List<Item> items, [FromBody]DateTime startDate, [FromBody]DateTime endDate)
        {
            var client = _clientService.GetClientByIdAsync(clientId);
            if (client == null)
            {
                _logger.LogWarning("Client with ID: {ClientId} not found", clientId);
                return new NotFoundResult();
            }

            ContractStatus status = ContractStatus.Inactive;

            if (startDate == DateTime.Now)
            {
                status = ContractStatus.Active;
            }

            var contract = await _contractService.AddContract(startDate,endDate,status);

            _clientService.AddContractToClient(clientId, contract);
            _logger.LogInformation("Added contract with ID: {ContractId} to client with ID: {ClientId}", contract.Id, clientId);
            return new OkObjectResult(contract);
        }

        [HttpPost("terminate")]
        public async Task<IActionResult> TerminateContract([FromQuery]int clientId, [FromQuery]int contractId)
        {
            var client = _clientService.GetClientByIdAsync(clientId);
            if (client == null)
            {
                _logger.LogWarning("Client with ID: {ClientId} not found", clientId);
                return new NotFoundResult();
            }
            if (_contractService.GetContractByIdAsync(contractId) == null)
            {
                _logger.LogWarning("Contract with ID: {ContractId} not found for client with ID: {ClientId}", contractId, clientId);
                return new NotFoundResult();
            }

            _clientService.SetClientContractStatus(clientId, contractId, ContractStatus.Terminated);
            _logger.LogInformation("Terminated contract with ID: {ContractId} for client with ID: {ClientId}", contractId, clientId);
            return new OkResult();
        }

        [HttpPost("complete")]
        public IActionResult CompleteContract([FromQuery]int clientId, [FromQuery]int contractId)
        {
            var client = _clientService.GetClientByIdAsync(clientId);
            if (client == null)
            {
                _logger.LogWarning("Client with ID: {ClientId} not found", clientId);
                return new NotFoundResult();
            }
            if (_contractService.GetContractByIdAsync(contractId) == null)
            {
                _logger.LogWarning("Contract with ID: {ContractId} not found for client with ID: {ClientId}", contractId, clientId);
                return new NotFoundResult();
            }

            _clientService.SetClientContractStatus(clientId,contractId, ContractStatus.Completed);
            _logger.LogInformation("Completed contract with ID: {ContractId} for client with ID: {ClientId}", contractId, clientId);
            return new OkResult();
        }
    }
}
