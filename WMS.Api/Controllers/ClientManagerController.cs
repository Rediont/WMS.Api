
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Services.Interfaces;
using System.Threading.Tasks;

namespace WMS.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientManagerController
    {
        private readonly IClientService _clientService;
        private readonly ILogger<ClientManagerController> _logger;

        public ClientManagerController(
            IClientService clientService, 
            ILogger<ClientManagerController> logger)
        {
            _clientService = clientService;
            _logger = logger;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllClients()
        {
            try
            {
                List<Client> clients = _clientService.GetAllClients();
                _logger.LogInformation("Retrieved {ClientCount} clients", clients.Count());
                return new OkObjectResult(clients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving clients");
                return new StatusCodeResult(500);
            }
        }

        [HttpGet("get/{clientId}")]
        public IActionResult GetClientById([FromRoute]Guid clientId)
        {
            try
            {
                var client = _clientService.GetClientByIdAsync(TODO);
                if (client == null)
                {
                    _logger.LogWarning("Client with ID: {ClientId} not found", clientId);
                    return new NotFoundResult();
                }
                _logger.LogInformation("Retrieved client with ID: {ClientId}", clientId);
                return new OkObjectResult(client);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving client with ID: {ClientId}", clientId);
                return new StatusCodeResult(500);
            }
        }


        // треба перевірити |
        //                  v


        [HttpPost("add")]
        public IActionResult AddClient(
            [FromForm]string clientName, 
            [FromForm]string emailAddress, 
            [FromForm]string contactPersonName, 
            [FromForm]string contactPersonPhone)
        {
            Client client = new Client
            {
                Name = clientName,
                Email = emailAddress,
                ContactPersonName = contactPersonName,
                ContactPersonPhone = contactPersonPhone,
                ContractList = new List<Contract>()
            };

            try
            {
                _clientRepository.AddClient(client);
                _logger.LogInformation("Added new client: {ClientName} with ID: {ClientId}", client.name, client.id);
                return new OkResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding client: {ClientName}", client.name);
                return new StatusCodeResult(500);
            }
        }

        [HttpDelete("remove")]
        public IActionResult RemoveClient(Guid clientId)
        {
            try
            {
                _clientRepository.RemoveClient(clientId);
                _logger.LogInformation("Removed client with ID: {ClientId}", clientId);
                return new OkResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing client with ID: {ClientId}", clientId);
                return new StatusCodeResult(500);
            }

        }
    }
}

