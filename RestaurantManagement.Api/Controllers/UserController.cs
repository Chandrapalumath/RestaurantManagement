using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Api.Middlewares;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.Dtos.Settings;
using RestaurantManagement.Dtos.Users;
using System.Security.Claims;

namespace RestaurantManagement.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ISettingsService _settingsService;

        public UserController(IUserService userService, ISettingsService settingsService)
        {
            _userService = userService;
            _settingsService = settingsService;
        }

        [HttpPost("users")]
        [ProducesResponseType(typeof(void), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUserAsync(CreateUserRequestDto dto)
        {
            var result = await _userService.CreateUserAsync(dto);
            return CreatedAtRoute("GetUserByIdAsync", new { id = result.Id }, null);
        }

        [HttpGet("users")]
        [ProducesResponseType(typeof(IEnumerable<UserResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            return Ok(await _userService.GetAllUsersAsync());
        }

        [HttpPatch("users/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUserAsync(Guid id, UserUpdateRequestDto dto)
        {
            await _userService.UpdateUserAsync(id, dto);
            return NoContent();
        }

        [Authorize]
        [HttpGet("users/{id}", Name = "GetUserByIdAsync")]
        [ProducesResponseType(typeof(IEnumerable<UserResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserByIdAsync(Guid id)
        {
            Guid? userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            bool isAdmin = User.IsInRole("Admin");
            return Ok(await _userService.GetUserByIdAsync(id, isAdmin, userId));
        }

        [HttpGet("settings")]
        [ProducesResponseType(typeof(IEnumerable<SettingsResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSettingsAsync()
        {
            return Ok(await _settingsService.GetSettingsAsync());
        }

        [HttpPatch("settings")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateSettingsAsync(SettingsUpdateRequestDto dto)
        {
            await _settingsService.UpdateSettingsAsync(dto);
            return NoContent();
        }
    }
}
