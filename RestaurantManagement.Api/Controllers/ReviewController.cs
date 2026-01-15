using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Backend.Services.Interfaces;
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
        public async Task<IActionResult> CreateReviewAsync(ReviewCreateRequestDto dto)
        {
            return Ok(await _reviewService.CreateAsync(dto));
        }
        [HttpGet]
        public async Task<IActionResult> GetAllReviewsAsync()
        {
            return Ok(await _reviewService.GetAllAsync());
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetReviewByIdAsync(int id)
        {
            return Ok(await _reviewService.GetByIdAsync(id));
        }
        [HttpGet("customer/{customerId:int}")]
        public async Task<IActionResult> GetReviewsByCustomerIdAsync(int customerId)
        {
            return Ok(await _reviewService.GetByCustomerIdAsync(customerId));
        }
    }
}
