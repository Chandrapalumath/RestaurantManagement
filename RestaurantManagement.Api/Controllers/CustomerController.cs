using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Api.Middlewares;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.Dtos.Billing;
using RestaurantManagement.Dtos.Customers;
using RestaurantManagement.Dtos.Pagination;

namespace RestaurantManagement.Api.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [Authorize(Roles = "Waiter")]
        [HttpPost]
        [ProducesResponseType(typeof(void), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateCustomerAsync(CustomerCreateRequestDto dto)
        {
            var customer = await _customerService.CreateAsync(dto);
            return CreatedAtRoute("GetCustomerById", new { id = customer.Id }, null);
        }

        [Authorize(Roles = "Admin,Waiter")]
        [HttpGet("{id}", Name = "GetCustomerById")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(CustomerResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCustomerByIdAsync(Guid id)
        {
            return Ok(await _customerService.GetByIdAsync(id)); ;
        }
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(PagedResult<CustomerResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllCustomersAsync(int page = 1, int pageSize = 5, string? search = null)
        {
            var result = await _customerService.GetAllAsync(page, pageSize, search);
            return Ok(result);
        }
        [Authorize(Roles = "Admin,Waiter")]
        [HttpGet("mobile/{number}")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(List<CustomerResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> GetCustomerByMobilePartialSearch(string number)
        {
            var customers = await _customerService.GetByMobileNumberAsync(number);

            if (!customers.Any())
                return Ok(new List<CustomerResponseDto>());

            return Ok(customers);
        }
    }
}