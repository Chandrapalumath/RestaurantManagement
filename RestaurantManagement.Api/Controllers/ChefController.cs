using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Api.Middlewares;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.Dtos.Billing;
using RestaurantManagement.Dtos.Orders;

namespace RestaurantManagement.Api.Controllers
{
    [Route("api/chef")]
    [ApiController]
    public class ChefController : Controller
    {
        private readonly IChefService _chefService;

        public ChefController(IChefService chefService)
        {
            _chefService = chefService;
        }

        [HttpGet("orders")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllOrdersForChefAsync([FromQuery] string? status)
        {
            return Ok(await _chefService.GetOrdersAsync(status));
        }
        [HttpGet("orders/{orderId:int}")]
        public async Task<IActionResult> GetOrderDetailsAsync(int orderId)
        {
            return Ok(await _chefService.GetOrderDetailsAsync(orderId));
        }
        [HttpPut("orders/{orderId:int}/statusCCHANGEIT")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateOrderStatusAsync(int orderId, OrderStatusUpdateRequestDto dto)
        {
            return Ok(await _chefService.UpdateOrderStatusAsync(orderId, dto));
        }
    }
}
