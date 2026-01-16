using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Api.Middlewares;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.Dtos.Settings;
using RestaurantManagement.Dtos.Users;

namespace RestaurantManagement.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/admin")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly ISettingsService _settingsService;

        public AdminController(IUserService userService, ISettingsService settingsService)
        {
            _userService = userService;
            _settingsService = settingsService;
        }

        [HttpPost("users")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateUserAsync(CreateUserRequestDto dto)
        {
            var result = await _userService.CreateStaffUserAsync(dto);
            return Ok(result);
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            return Ok(await _userService.GetAllUsersAsync());
        }

        [HttpGet("users/{id:int}")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserByIdAsync(int id)
        {
            return Ok(await _userService.GetUserByIdAsync(id));
        }

        [HttpPatch("users/{id:int}")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateUserAsync(int id, [FromBody] UserUpdateRequestDto dto)
        {
            var message = await _userService.UpdateUserAsync(id, dto);
            return Ok(message);
        }

        [HttpGet("settings")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSettingsAsync()
        {
            return Ok(await _settingsService.GetSettingsAsync());
        }

        [HttpPatch("settings")]
        public async Task<IActionResult> UpdateSettingsAsync(SettingsUpdateRequestDto dto)
        {
            return Ok(await _settingsService.UpdateSettingsAsync(dto));
        }

    }
}
