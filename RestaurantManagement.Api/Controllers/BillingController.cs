using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Api.Middlewares;
using RestaurantManagement.Backend.Services.Interfaces;
using System.Security.Claims;

namespace RestaurantManagement.Api.Controllers
{
    [Route("api/billing")]
    [ApiController]
    public class BillingController : Controller
    {
        private readonly IBillingService _billingService;

        public BillingController(IBillingService billingService)
        {
            _billingService = billingService;
        }

        [HttpPost("generate/order/{customerId:int}")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GenerateBill(int customerId)
        {
            int waiterId = 1;
            var result = await _billingService.GenerateBillAsync(customerId, waiterId);
            return Ok(result);
        }

        [HttpPut("{billId:int}/payment-done")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> MarkPaymentDoneAsync(int billId)
        {
            int waiterId = 1; // hc value change at the time of authorization
            await _billingService.MarkPaymentDoneAsync(billId, waiterId);
            return Ok("Payment marked as done.");
        }

        [HttpGet("{billId:int}")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBillByIdAsync(int billId)
        {
            int? waiterId = User.IsInRole("Waiter")
            ? int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)
            : null;

            bool isAdmin = User.IsInRole("Admin");

            return Ok(await _billingService.GetBillByIdAsync(billId, waiterId, isAdmin));
        }

        [HttpGet("customer/{customerId:int}")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBillsByCustomerIdAsync(int customerId)
        {
            int? waiterId = User.IsInRole("Waiter")
            ? int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)
            : null;

            bool isAdmin = User.IsInRole("Admin");

            return Ok(await _billingService.GetBillsByCustomerIdAsync(customerId, waiterId, isAdmin));
        }

        [HttpGet]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBillsAsync()
        {
            return Ok(await _billingService.GetAllBillsAsync());
        }

    }
}
