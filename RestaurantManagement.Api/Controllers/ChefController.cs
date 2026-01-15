using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Backend.Services.Interfaces;
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
        public async Task<IActionResult> UpdateOrderStatusAsync(int orderId, OrderStatusUpdateRequestDto dto)
        {
            return Ok(await _chefService.UpdateOrderStatusAsync(orderId, dto));
        }
    }
}
