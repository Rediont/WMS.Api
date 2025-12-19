using Core.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WMS.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController
    {
        private readonly IItemRepository _itemRepository;
        private readonly ILogger<ItemController> _logger;

        public ItemController(
            IItemRepository itemService,
            ILogger<ItemController> logger)
        {
            _itemRepository = itemService;
            _logger = logger;
        }

        [HttpGet("all")]
        public IActionResult GetAllItems()
        {
            try
            {
                var items = _itemRepository.GetAllItems();
                _logger.LogInformation("Retrieved {ItemCount} items", items.Count());
                return new OkObjectResult(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving items");
                return new StatusCodeResult(500);
            }
        }

        [HttpGet("contract")]
        public IActionResult GetItemsByContractId([FromQuery]string contractId)
        {
            try
            {
                var items = _itemRepository.GetItemsByContractId(contractId);
                _logger.LogInformation("Retrieved {ItemCount} items for contract ID: {ContractId}", items.Count(), contractId);
                return new OkObjectResult(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving items for contract ID: {ContractId}", contractId);
                return new StatusCodeResult(500);
            }
        }

        [HttpGet("client")]
        public IActionResult GetItemsByClientId([FromQuery]Guid clientId)
        {
            try
            {
                var items = _itemRepository.GetItemsByClientId(clientId);
                _logger.LogInformation("Retrieved {ItemCount} items for client ID: {ClientId}", items.Count(), clientId);
                return new OkObjectResult(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving items for client ID: {ClientId}", clientId);
                return new StatusCodeResult(500);
            }
        }

        [HttpPost("add")]
        public IActionResult AddItem([FromBody] Item item)
        {
            try
            {
                _itemRepository.AddItem(item);
                _logger.LogInformation("Added new item with ID: {ItemId}", item.Id);
                return new OkResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new item");
                return new StatusCodeResult(500);
            }
        }

        [HttpDelete("remove/{itemId}")]
        public IActionResult RemoveItem(int itemId)
        {
            try
            {
                _itemRepository.RemoveItem(itemId);
                _logger.LogInformation("Removed item with ID: {ItemId}", itemId);
                return new OkResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing item with ID: {ItemId}", itemId);
                return new StatusCodeResult(500);
            }
        }

    }
}
