using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Api.Controllers
{
    [ApiController]
    public class InventoryController
    {
        private readonly IPalletService _inventoryService;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(IPalletService inventoryService, ILogger<InventoryController> logger)
        {
            _inventoryService = inventoryService;
            _logger = logger;
        }



    }
}
