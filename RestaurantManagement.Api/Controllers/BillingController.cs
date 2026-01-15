using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [HttpPost("generate/order/{orderId:int}")]
        public async Task<IActionResult> GenerateBillByOrderIdAsync(int orderId)
        {
            int waiterId = 1;
            var result = await _billingService.GenerateBillByOrderIdAsync(orderId, waiterId);
            return Ok(result);
        }

        [HttpPut("{billId:int}/payment-done")]
        public async Task<IActionResult> MarkPaymentDoneAsync(int billId)
        {
            int waiterId = 1; // hc value change at the time of authorization
            await _billingService.MarkPaymentDoneAsync(billId, waiterId);
            return Ok("Payment marked as done.");
        }

        [HttpGet("{billId:int}")]
        public async Task<IActionResult> GetBillByIdAsync(int billId)
        {
            int? waiterId = User.IsInRole("Waiter")
            ? int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)
            : null;

            bool isAdmin = User.IsInRole("Admin");

            return Ok(await _billingService.GetBillByIdAsync(billId, waiterId, isAdmin));
        }

        [HttpGet("customer/{customerId:int}")]
        public async Task<IActionResult> GetBillsByCustomerIdAsync(int customerId)
        {
            int? waiterId = User.IsInRole("Waiter")
            ? int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)
            : null;

            bool isAdmin = User.IsInRole("Admin");

            return Ok(await _billingService.GetBillsByCustomerIdAsync(customerId, waiterId, isAdmin));
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBillsAsync()
        {
            return Ok(await _billingService.GetAllBillsAsync());
        }

    }
}
