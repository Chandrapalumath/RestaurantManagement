using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Repositories.Interfaces;
using RestaurantManagement.Dtos.Reviews;

namespace RestaurantManagement.Backend.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepo;
        private readonly ICustomerRepository _customerRepo;

        public ReviewService(IReviewRepository reviewRepo, ICustomerRepository customerRepo)
        {
            _reviewRepo = reviewRepo;
            _customerRepo = customerRepo;
        }

        public async Task<ReviewResponseDto> CreateAsync(ReviewCreateRequestDto dto)
        {
            var customer = await _customerRepo.GetByIdAsync(dto.CustomerId);
            if (customer == null)
                throw new Exception("Customer not found.");

            var review = new Review
            {
                CustomerId = dto.CustomerId,
                Rating = dto.Rating,
                Comment = dto.Comment?.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            await _reviewRepo.AddAsync(review);
            await _reviewRepo.SaveChangesAsync();

            return MapReviewToDto(review);
        }

        public async Task<List<ReviewResponseDto>> GetAllAsync()
        {
            var reviews = await _reviewRepo.GetAllAsync();
            return reviews.Select(MapReviewToDto).ToList();
        }

        public async Task<ReviewResponseDto> GetByIdAsync(int id)
        {
            var review = await _reviewRepo.GetByIdAsync(id)
                         ?? throw new Exception("Review not found.");

            return MapReviewToDto(review);
        }

        public async Task<List<ReviewResponseDto>> GetByCustomerIdAsync(int customerId)
        {
            var reviews = await _reviewRepo.GetByCustomerIdAsync(customerId);
            return reviews.Select(MapReviewToDto).ToList();
        }

        private static ReviewResponseDto MapReviewToDto(Review review)
        {
            return new ReviewResponseDto
            {
                ReviewId = review.Id,
                CustomerId = review.CustomerId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt
            };
        }
    }

}