using RestaurantManagement.Backend.Exceptions;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Repositories.Interfaces;
using RestaurantManagement.Dtos.Pagination;
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
                throw new NotFoundException("Customer not found.");

            var review = new Review
            {
                Id = Guid.NewGuid(),
                CustomerId = dto.CustomerId,
                Rating = dto.Rating,
                Comment = dto.Comment?.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            await _reviewRepo.AddAsync(review);
            await _reviewRepo.SaveChangesAsync();

            return MapReviewToDto(review);
        }

        public async Task<PagedResult<ReviewResponseDto>> GetAllAsync(int page, int pageSize, string? search)
        {
            var reviews = await _reviewRepo.GetAllAsync();
            var customers = await _customerRepo.GetAllAsync();

            var customerMap = customers.ToDictionary(c => c.Id, c => c.Name);

            var mapped = reviews.Select(r => new ReviewResponseDto
            {
                ReviewId = r.Id,
                CustomerId = r.CustomerId,
                CustomerName = customerMap.ContainsKey(r.CustomerId) ? customerMap[r.CustomerId] : "Unknown",
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            }).ToList();

            if (!string.IsNullOrWhiteSpace(search))
                mapped = mapped.Where(r =>
                    r.CustomerName.ToLower().Contains(search.ToLower()) ||
                    (r.Comment != null && r.Comment.ToLower().Contains(search.ToLower()))
                ).ToList();

            var total = mapped.Count;

            var paged = mapped
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<ReviewResponseDto>
            {
                Items = paged,
                TotalCount = total
            };
        }

        public async Task<ReviewResponseDto> GetByIdAsync(Guid id)
        {
            var review = await _reviewRepo.GetByIdAsync(id)
                         ?? throw new NotFoundException("Review not found.");

            return MapReviewToDto(review);
        }

        public async Task<List<ReviewResponseDto>> GetByCustomerIdAsync(Guid customerId)
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