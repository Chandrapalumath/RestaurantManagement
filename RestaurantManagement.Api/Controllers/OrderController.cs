using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Api.Middlewares;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.Dtos.Billing;
using RestaurantManagement.Dtos.Orders;
using RestaurantManagement.Models.Common.Enums;
using System.Security.Claims;

namespace RestaurantManagement.Api.Controllers
{
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [Authorize(Roles = "Waiter")]
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
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
            Guid waiterId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); 
            return Ok(await _orderService.CreateOrderAsync(dto, waiterId));
        }

        [Authorize(Roles = "Waiter,Chef")]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrderByIdAsync(Guid id)
        {
            return Ok(await _orderService.GetByIdAsync(id));
        }

        [HttpGet("customer/{id}")]
        public async Task<IActionResult> GetOrdersByCustomerIdAsync(Guid id)
        {
            return Ok(await _orderService.GetOrdersByCustomerIdAsync(id));
        }
        
        [HttpGet("status")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllOrdersForStatusAsync(OrderStatus status)
        {
            return Ok(await _orderService.GetOrdersAsync(status));
        }
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateOrderStatusAsync(Guid id, OrderUpdateRequestDto dto)
        {
            return Ok(await _orderService.UpdateOrderAsync(id, dto));
        }
    }
}
