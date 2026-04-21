using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos;
using Services.Interfaces;

namespace WMS.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize()]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWarehouseSettingsService _warehouseSettingsService;
        private readonly IConfiguration _configuration;

        public AdminController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IWarehouseSettingsService warehouseSettingsService, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _warehouseSettingsService = warehouseSettingsService;
            _configuration = configuration;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!await _roleManager.RoleExistsAsync(model.Role))
            {
                return BadRequest($"Роль '{model.Role}' не існує.");
            }

            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                return BadRequest("Користувач з таким Email вже існує.");
            }

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // 4. Призначаємо йому роль
                await _userManager.AddToRoleAsync(user, model.Role);
                return Ok($"Користувач {model.Email} успішно створений з роллю {model.Role}.");
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("update-warehouse-settings")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateWarehouseSettings([FromBody] WarehouseSettingsDto settings)
        {

            var updatedSettings = await _warehouseSettingsService.UpdateWarehouseSettingsAsync(
                settings.NumberOfAlleys,
                settings.NumberOfFloorsPerAlley,
                settings.CellsPerAlleyFloor);

            return Ok(new
            {
                Message = "Налаштування складу успішно оновлено",
                Settings = updatedSettings
            });
        }

        [HttpGet("get-warehouse-settings")]
        [Authorize]
        public async Task<IActionResult> GetWarehouseSettings()
        {
            var settings = await _warehouseSettingsService.GetWarehouseSettingsAsync();
            return Ok(settings);
        }

    }
}
