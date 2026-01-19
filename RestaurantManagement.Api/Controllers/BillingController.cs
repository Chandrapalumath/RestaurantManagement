using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Api.Middlewares;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.Dtos.Billing;
using System.Security.Claims;

namespace RestaurantManagement.Api.Controllers
{
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [Authorize(Roles = "Waiter,Admin")]
    [Route("api/billing")]
    [ApiController]
    public class BillingController : ControllerBase
    {
        private readonly IBillingService _billingService;

        public BillingController(IBillingService billingService)
        {
            _billingService = billingService;
        }

        [Authorize(Roles = "Waiter")]
        [HttpPost("customer/{id}")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GenerateBill(Guid id)
        {
            Guid waiterId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _billingService.GenerateBillAsync(id, waiterId);
            return Ok(result);
        }

        [Authorize(Roles = "Waiter")]
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateBill(Guid id, BillUpdateRequestDto dto)
        {
            Guid waiterId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _billingService.UpdateBill(id, waiterId, dto);
            return Ok("Updated Successfully");
        }

        [Authorize(Roles = "Waiter,Admin")]
        [HttpGet("{id:Guid}")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBillByIdAsync(Guid id)
        {
            Guid? waiterId = User.IsInRole("Waiter")
            ? Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)
            : null;

            bool isAdmin = User.IsInRole("Admin");

            return Ok(await _billingService.GetBillByIdAsync(id, waiterId, isAdmin));
        }

        [HttpGet("customer/{id:Guid}")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBillsByCustomerIdAsync(Guid id)
        {
            Guid? waiterId = User.IsInRole("Waiter")
            ? Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)
            : null;

            bool isAdmin = User.IsInRole("Admin");

            return Ok(await _billingService.GetBillsByCustomerIdAsync(id, waiterId, isAdmin));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBillsAsync()
        {
            return Ok(await _billingService.GetAllBillsAsync());
        }
    }
}
