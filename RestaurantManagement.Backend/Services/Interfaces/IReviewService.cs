using RestaurantManagement.Dtos.Pagination;
using RestaurantManagement.Dtos.Reviews;

namespace RestaurantManagement.Backend.Services.Interfaces
{
    public interface IReviewService
    {
        Task<ReviewResponseDto> CreateAsync(ReviewCreateRequestDto dto);
        Task<PagedResult<ReviewResponseDto>> GetAllAsync(int page, int pageSize, string? search);
        Task<ReviewResponseDto> GetByIdAsync(Guid id);
        Task<List<ReviewResponseDto>> GetByCustomerIdAsync(Guid customerId);
    }
}
