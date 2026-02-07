using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Api.Middlewares;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.Dtos.Pagination;
using RestaurantManagement.Dtos.Reviews;

namespace RestaurantManagement.Api.Controllers
{
    [Authorize]
    [Route("api/reviews")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [Authorize(Roles = "Waiter")]
        [HttpPost]
        [ProducesResponseType(typeof(void), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateReviewAsync(ReviewCreateRequestDto dto)
        {
            var review = await _reviewService.CreateAsync(dto);
            return CreatedAtRoute("GetReviewById", new { id = review.ReviewId }, null);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(PagedResult<ReviewResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllReviewsAsync(int page = 1, int pageSize = 5, string? search = null)
        {
            var result = await _reviewService.GetAllAsync(page, pageSize, search);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}", Name = "GetReviewById")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ReviewResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReviewByIdAsync(Guid id)
        {
            return Ok(await _reviewService.GetByIdAsync(id));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("customer/{id}")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(List<ReviewResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReviewsByCustomerIdAsync(Guid id)
        {
            return Ok(await _reviewService.GetByCustomerIdAsync(id));
        }
    }
}
