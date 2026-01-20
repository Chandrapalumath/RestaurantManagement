using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Api.Middlewares;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.Dtos.Billing;
using System.Security.Claims;

namespace RestaurantManagement.Api.Controllers
{
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
        [HttpPost]
        [ProducesResponseType(typeof(BillResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GenerateBill(BillGenerateRequestDto dto)
        {
            Guid waiterId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _billingService.GenerateBillAsync(waiterId, dto);
            return CreatedAtRoute("GetBillById", new { id = result.BillId }, null);
        }

        [Authorize(Roles = "Waiter")]
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateBill(Guid id, BillUpdateRequestDto dto)
        {
            Guid waiterId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _billingService.UpdateBill(id, waiterId, dto);
            return NoContent();
        }

        [Authorize(Roles = "Waiter,Admin")]
        [HttpGet("{id:Guid}", Name = "GetBillById")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(BillResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetBillByIdAsync(Guid id)
        {
            Guid? waiterId = User.IsInRole("Waiter")
            ? Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)
            : null;

            bool isAdmin = User.IsInRole("Admin");

            return Ok(await _billingService.GetBillByIdAsync(id, waiterId, isAdmin));
        }

        //[Authorize(Roles = "Waiter,Admin")]
        //[HttpGet("customer/{id:Guid}")]
        //[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        //[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        //[ProducesResponseType(typeof(BillResponseDto), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        //[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        //public async Task<IActionResult> GetBillsByCustomerIdAsync(Guid id)
        //{
        //    Guid? waiterId = User.IsInRole("Waiter")
        //    ? Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)
        //    : null;

        //    bool isAdmin = User.IsInRole("Admin");

        //    return Ok(await _billingService.GetBillsByCustomerIdAsync(id, waiterId, isAdmin));
        //}

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(List<BillResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllBillsAsync()
        {
            return Ok(await _billingService.GetAllBillsAsync());
        }
    }
}
