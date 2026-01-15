using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Api.Middlewares;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.Dtos.Billing;
using RestaurantManagement.Dtos.Menu;

namespace RestaurantManagement.Api.Controllers
{
    [Route("api/menu")]
    [ApiController]
    public class MenuController : Controller
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllMenuItemsAsync()
        {
            return Ok(await _menuService.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMenuItemByIdAsync(int id)
        {
            return Ok(await _menuService.GetByIdAsync(id)); ;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMenuItemAsync(MenuItemCreateRequestDto dto)
        {
            return Ok(await _menuService.CreateAsync(dto));
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateMenuItemAsync(int id, MenuItemUpdateRequestDto dto)
        {
            return Ok(await _menuService.UpdateAsync(id, dto));
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteMenuItemAsync(int id)
        {
            await _menuService.DeleteAsync(id);
            return Ok("Menu item deleted.");
        }
    }
}
