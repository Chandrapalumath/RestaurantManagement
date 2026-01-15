using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Api.Middlewares;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.Dtos.Billing;
using RestaurantManagement.Dtos.Reviews;

namespace RestaurantManagement.Api.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateReviewAsync(ReviewCreateRequestDto dto)
        {
            return Ok(await _reviewService.CreateAsync(dto));
        }

        [HttpGet]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllReviewsAsync()
        {
            return Ok(await _reviewService.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReviewByIdAsync(int id)
        {
            return Ok(await _reviewService.GetByIdAsync(id));
        }

        [HttpGet("customer/{customerId:int}")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReviewsByCustomerIdAsync(int customerId)
        {
            return Ok(await _reviewService.GetByCustomerIdAsync(customerId));
        }
    }
}
