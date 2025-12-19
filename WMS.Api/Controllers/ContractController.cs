using Core.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WMS.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContractController
    {
        private readonly IContractRepository _contractRepository;

        private readonly IClientRepository _clientRepository;

        private readonly ILogger<ContractController> _logger;

        public ContractController(
            IContractRepository contractRepository,
            IClientRepository clientRepository,
            ILogger<ContractController> logger)
        {
            _contractRepository = contractRepository;
            _clientRepository = clientRepository;
            _logger = logger;
        }

        [HttpGet("all")]
        public IActionResult GetAllContracts()
        {
            try
            {
                var contracts = _contractRepository.GetAllContracts();
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
        public IActionResult AddContract([FromQuery]Guid clientId, [FromBody]List<Item> items, [FromBody]DateTime startDate, [FromBody]DateTime endDate)
        {
            var client = _clientRepository.GetClientById(TODO);
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

            Contract contract = new Contract
            {
                item_list = items,
                start_date = startDate,
                expiration_date = endDate,
                current_status = status

            };

            _clientRepository.AddContractToClient(clientId, contract);
            _logger.LogInformation("Added contract with ID: {ContractId} to client with ID: {ClientId}", contract.id, clientId);
            return new OkObjectResult(contract);
        }

        [HttpPost("terminate")]
        public IActionResult TerminateContract([FromQuery]Guid clientId, [FromQuery]string contractId)
        {
            var client = _clientRepository.GetClientById(TODO);
            if (client == null)
            {
                _logger.LogWarning("Client with ID: {ClientId} not found", clientId);
                return new NotFoundResult();
            }
            if (_contractRepository.GetContractById(contractId, clientId) == null)
            {
                _logger.LogWarning("Contract with ID: {ContractId} not found for client with ID: {ClientId}", contractId, clientId);
                return new NotFoundResult();
            }

            _clientRepository.TerminateClientContract(clientId, contractId);
            _logger.LogInformation("Terminated contract with ID: {ContractId} for client with ID: {ClientId}", contractId, clientId);
            return new OkResult();
        }

        [HttpPost("complete")]
        public IActionResult CompleteContract([FromQuery]Guid clientId, [FromQuery]string contractId)
        {
            var client = _clientRepository.GetClientById(TODO);
            if (client == null)
            {
                _logger.LogWarning("Client with ID: {ClientId} not found", clientId);
                return new NotFoundResult();
            }
            if (_contractRepository.GetContractById(contractId, clientId) == null)
            {
                _logger.LogWarning("Contract with ID: {ContractId} not found for client with ID: {ClientId}", contractId, clientId);
                return new NotFoundResult();
            }

            _contractRepository.SetContractStatusToCompleted(contractId);
            _logger.LogInformation("Completed contract with ID: {ContractId} for client with ID: {ClientId}", contractId, clientId);
            return new OkResult();
        }

        [HttpPost("delivery")]
        public IActionResult AddDeliveryToContract([FromQuery] Guid clientId, [FromQuery]string contractId, [FromBody]List<Item> items, [FromBody]DateTime? arrivalDate)
        {
            var client = _clientRepository.GetClientById(TODO);
            if (client == null)
            {
                _logger.LogWarning("Client with ID: {ClientId} not found", clientId);
                return new NotFoundResult();
            }
            var contract = _contractRepository.GetContractById(contractId, clientId);
            if (contract == null)
            {
                _logger.LogWarning("Contract with ID: {ContractId} not found for client with ID: {ClientId}", contractId, clientId);
                return new NotFoundResult();
            }

            ContractDelivery delivery = new ContractDelivery
            {
                contract_id = contractId,
                item_list = items,
                arrival_date = arrivalDate ?? DateTime.Now
            };

            _contractRepository.AddShipmentToContract(contractId, delivery);
            _logger.LogInformation("Added shipment to contract with ID: {ContractId} for client with ID: {ClientId}", contractId, clientId);
            return new OkResult();
        }

        [HttpPost("shipment")]
        public IActionResult AddShipmentToContract([FromQuery] Guid clientId, [FromQuery]string contractId, [FromBody]List<Item> items, [FromBody]DateTime? shipmentDate)
        {
            var client = _clientRepository.GetClientById(TODO);
            if (client == null)
            {
                _logger.LogWarning("Client with ID: {ClientId} not found", clientId);
                return new NotFoundResult();
            }
            var contract = _contractRepository.GetContractById(contractId, clientId);
            if (contract == null)
            {
                _logger.LogWarning("Contract with ID: {ContractId} not found for client with ID: {ClientId}", contractId, clientId);
                return new NotFoundResult();
            }
            ContractShipment shipment = new ContractShipment
            {
                contract_id = contractId,
                item_list = items,
                shipment_date = shipmentDate ?? DateTime.Now
            };
            _contractRepository.AddDeliveryToContract(contractId, shipment);
            _logger.LogInformation("Added delivery to contract with ID: {ContractId} for client with ID: {ClientId}", contractId, clientId);
            return new OkResult();

        }
    }
}
