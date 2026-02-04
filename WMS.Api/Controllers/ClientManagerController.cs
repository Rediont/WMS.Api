
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Services.Interfaces;
using System.Threading.Tasks;
using Services.Dtos;

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
                var clients = await _clientService.GetAllClients();
                _logger.LogInformation("Retrieved {ClientCount} clients", clients.Count());
                return new OkObjectResult(clients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving clients");
                return new StatusCodeResult(500);
            }
        }

        [HttpGet("{clientId}")]
        public async Task<IActionResult> GetClientById([FromRoute] int clientId)
        {
            try
            {
                var client = await _clientService.GetClientByIdAsync(clientId);
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
        public async Task<IActionResult> AddClient(
            [FromForm] string clientName,
            [FromForm] string emailAddress,
            [FromForm] string EDRPO,
            [FromForm] string contactPersonName,
            [FromForm] string contactPersonPhone)
        {
            try
            {
                await _clientService.AddClient(clientName, EDRPO, contactPersonName, contactPersonPhone, emailAddress);
                _logger.LogInformation("Added new client: {ClientName}", clientName);
                return new OkResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding client: {ClientName}", clientName);
                return new StatusCodeResult(500);
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> RemoveClient([FromQuery]int clientId)
        {
            try
            {
                await _clientService.DeleteClient(clientId);
                _logger.LogInformation("Removed client with ID: {ClientId}", clientId);
                return new OkResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing client with ID: {ClientId}", clientId);
                return new StatusCodeResult(500);
            }

        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateClient(
            [FromForm]int clientId,
            [FromForm] string? clientName = null,
            [FromForm] string? emailAddress = null,
            [FromForm] string? EDRPO = null,
            [FromForm] string? contactPersonName = null,
            [FromForm] string? contactPersonPhone = null)
        {
            try
            {
                await _clientService.UpdateClientAsync(
                    clientId,
                    clientName,
                    EDRPO,
                    contactPersonName,
                    contactPersonPhone,
                    emailAddress);
                _logger.LogInformation("Updated client with ID: {ClientId}", clientId);
                return new OkResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating client with ID: {ClientId}", clientId);
                return new StatusCodeResult(500);
            }

        }

        //====================================================================================
        //================================ не готове ! ========================================
        //================================           v =======================================
        //====================================================================================


        public Task<IActionResult> CalculateCostForClient()
        {
            throw new NotImplementedException();
        }



    }
}

