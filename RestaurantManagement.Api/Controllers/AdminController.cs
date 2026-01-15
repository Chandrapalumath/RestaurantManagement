using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.Dtos.Settings;
using RestaurantManagement.Dtos.Users;

namespace RestaurantManagement.Api.Controllers
{
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

        [HttpPost("user")]
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
        public async Task<IActionResult> GetUserByIdAsync(int id)
        {
            return Ok(await _userService.GetUserByIdAsync(id));
        }

        [HttpPatch("user/{id:int}")]
        public async Task<IActionResult> UpdateUserAsync(int id, [FromBody] UserUpdateRequestDto dto)
        {
            var message = await _userService.UpdateUserAsync(id, dto);
            return Ok(message);
        }
        //[HttpPut("users/{id:int}/activate")]
        //public async Task<IActionResult> ActivateUser(int id)
        //{
        //    await _userService.ActivateUserAsync(id);
        //    return Ok("User activated.");
        //}
        //[HttpPut("users/{id:int}/deactivate")]
        //public async Task<IActionResult> DeactivateUser(int id)
        //{
        //    await _userService.DeactivateUserAsync(id);
        //    return Ok("User deactivated.");
        //}
        [HttpGet("settings")]
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
