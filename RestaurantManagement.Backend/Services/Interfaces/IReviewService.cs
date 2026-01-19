using RestaurantManagement.Dtos.Reviews;

namespace RestaurantManagement.Backend.Services.Interfaces
{
    public interface IReviewService
    {
        Task<ReviewResponseDto> CreateAsync(ReviewCreateRequestDto dto);
        Task<List<ReviewResponseDto>> GetAllAsync();
        Task<ReviewResponseDto> GetByIdAsync(Guid id);
        Task<List<ReviewResponseDto>> GetByCustomerIdAsync(Guid customerId);
    }
}
