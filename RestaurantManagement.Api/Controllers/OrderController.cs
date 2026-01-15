using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.Dtos.Orders;
using System.Security.Claims;

namespace RestaurantManagement.Api.Controllers
{
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
        public async Task<IActionResult> CreateOrderAsync(OrderCreateRequestDto dto)
        {
            int waiterId = 1; // this value is hard coded change at the time of the authorization
            return Ok(await _orderService.CreateOrderAsync(dto, waiterId));
        }
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetOrderByIdAsync(int Id)
        {
            return Ok(await _orderService.GetByIdAsync(Id));
        }
        [HttpGet("customer/{customerId:int}")]
        public async Task<IActionResult> GetOrdersByCustomerIdAsync(int customerId)
        {
            return Ok(await _orderService.GetOrdersByCustomerIdAsync(customerId));
        }

    }
}
