using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Api.Middlewares;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.Dtos.Billing;
using System.Security.Claims;

namespace RestaurantManagement.Api.Controllers
{
    [Authorize(Roles = "Waiter,Admin")]
    [Route("api/bills")]
    [ApiController]
    public class BillingController : Controller
    {
        private readonly IBillingService _billingService;

        public BillingController(IBillingService billingService)
        {
            _billingService = billingService;
        }

        [Authorize(Roles = "Waiter")]
        [HttpPost("{Id:int}")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GenerateBill(int Id)
        {
            int waiterId = 1;
            var result = await _billingService.GenerateBillAsync(Id, waiterId);
            return Ok(result);
        }

        [Authorize(Roles = "Waiter")]
        [HttpPatch("{Id:int}")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateBill(int Id, BillUpdateRequestDto dto)
        {
            int waiterId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _billingService.UpdateBill(Id, waiterId, dto);
            return Ok("Updated Successfully");
        }

        [Authorize(Roles = "Waiter,Admin")]
        [HttpGet("{Id:int}")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBillByIdAsync(int Id)
        {
            int? waiterId = User.IsInRole("Waiter")
            ? int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)
            : null;

            bool isAdmin = User.IsInRole("Admin");

            return Ok(await _billingService.GetBillByIdAsync(Id, waiterId, isAdmin));
        }

        [HttpGet("customer/{Id:int}")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBillsByCustomerIdAsync(int Id)
        {
            int? waiterId = User.IsInRole("Waiter")
            ? int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)
            : null;

            bool isAdmin = User.IsInRole("Admin");

            return Ok(await _billingService.GetBillsByCustomerIdAsync(Id, waiterId, isAdmin));
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
