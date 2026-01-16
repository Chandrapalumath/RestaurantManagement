using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Api.Middlewares;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.Dtos.Billing;
using RestaurantManagement.Dtos.Orders;
using System.Security.Claims;

namespace RestaurantManagement.Api.Controllers
{
    [Authorize(Roles = "Waiter")]
    [Route("api/orders")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateOrderAsync(OrderCreateRequestDto dto)
        {
            int waiterId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); 
            return Ok(await _orderService.CreateOrderAsync(dto, waiterId));
        }

        [Authorize(Roles = "Waiter,Chef")]
        [HttpGet("{Id:int}")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrderByIdAsync(int Id)
        {
            return Ok(await _orderService.GetByIdAsync(Id));
        }

        [HttpGet("customer/{Id:int}")]
        public async Task<IActionResult> GetOrdersByCustomerIdAsync(int Id)
        {
            return Ok(await _orderService.GetOrdersByCustomerIdAsync(Id));
        }

    }
}
