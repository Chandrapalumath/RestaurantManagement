using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Api.Middlewares;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.Dtos.Billing;
using RestaurantManagement.Dtos.Customers;

namespace RestaurantManagement.Api.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CustomerCreateRequestDto dto)
        {
            return Ok(await _customerService.CreateAsync(dto));
        }
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCustomerByIdAsync(int id)
        {
            return Ok(await _customerService.GetByIdAsync(id)); ;
        }
        [HttpGet]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCustomersAsync()
        {
            return Ok(await _customerService.GetAllAsync()); ;
        }

    }
}
