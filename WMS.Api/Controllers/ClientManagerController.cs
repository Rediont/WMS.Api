
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Interfaces;
using Core.Entities;

namespace WMS.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientManagerController
    {
        private readonly IClientRepository _clientRepository;
        private readonly ILogger<ClientManagerController> _logger;

        public ClientManagerController(
            IClientRepository clientRepository, 
            ILogger<ClientManagerController> logger)
        {
            _clientRepository = clientRepository;
            _logger = logger;
        }

        [HttpGet("all")]
        public IActionResult GetAllClients()
        {
            try
            {
                var clients = _clientRepository.GetAllClients();
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
                var client = _clientRepository.GetClientById(clientId);
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

        [HttpPost("add")]
        public IActionResult AddClient(
            [FromForm]string clientName, 
            [FromForm]string emailAddress, 
            [FromForm]string contactPersonName, 
            [FromForm]string contactPersonPhone)
        {
            Client client = new Client
            {
                name = clientName,
                email = emailAddress,
                contact_person_name = contactPersonName,
                contact_phone = contactPersonPhone,
                Contract_list = new List<Contract>()
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

