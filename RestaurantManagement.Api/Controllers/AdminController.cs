using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Api.Middlewares;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.Dtos.Settings;
using RestaurantManagement.Dtos.Users;

namespace RestaurantManagement.Api.Controllers
{
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [Authorize(Roles = "Admin")]
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ISettingsService _settingsService;

        public AdminController(IUserService userService, ISettingsService settingsService)
        {
            _userService = userService;
            _settingsService = settingsService;
        }

        [HttpPost("users")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateUserAsync(CreateUserRequestDto dto)
        {
            var result = await _userService.CreateStaffUserAsync(dto);
            return Created();
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            return Ok(await _userService.GetAllUsersAsync());
        }

        [HttpGet("users/{id:int}")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserByIdAsync(int id)
        {
            return Ok(await _userService.GetUserByIdAsync(id));
        }

        [HttpPatch("users/{id:int}")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserAsync(int id, [FromBody] UserUpdateRequestDto dto)
        {
            var message = await _userService.UpdateUserAsync(id, dto);
            return Ok(message);
        }

        [HttpGet("settings")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
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
