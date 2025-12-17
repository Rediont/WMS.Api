using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WMS.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContractController
    {
        private readonly IContactRepository _contractRepository;

        private readonly ILogger<ContractController> _logger;

        public ContractController(
            IContactRepository contractRepository,
            ILogger<ContractController> logger)
        {
            _contractRepository = contractRepository;
            _logger = logger;
        }

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

    }
}
