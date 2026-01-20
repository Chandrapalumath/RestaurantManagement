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
        [ProducesResponseType(typeof(void), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrderAsync(OrderCreateRequestDto dto)
        {
            Guid waiterId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); 
            var order = await _orderService.CreateOrderAsync(dto, waiterId);
            return CreatedAtRoute("GetOrderById", new { id = order.OrderId }, null);
        }

        [Authorize(Roles = "Waiter,Chef")]
        [HttpGet("{id}", Name = "GetOrderById")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(OrderResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrderByIdAsync(Guid id)
        {
            return Ok(await _orderService.GetByIdAsync(id));
        }

        [HttpGet("table/{id}")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(List<OrderResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrdersByCustomerIdAsync(Guid id)
        {
            return Ok(await _orderService.GetOrdersByTableIdAsync(id));
        }

        [HttpGet("status")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(List<OrderResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllOrdersForStatusAsync(OrderStatus status)
        {
            return Ok(await _orderService.GetOrdersAsync(status));
        }
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateOrderStatusAsync(Guid id, OrderUpdateRequestDto dto)
        {
            await _orderService.UpdateOrderAsync(id, dto);
            return NoContent();
        }
    }
}
