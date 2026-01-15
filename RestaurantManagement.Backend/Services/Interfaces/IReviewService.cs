using RestaurantManagement.Dtos.Reviews;

namespace RestaurantManagement.Backend.Services.Interfaces
{
    public interface IReviewService
    {
        Task<ReviewResponseDto> CreateAsync(ReviewCreateRequestDto dto);
        Task<List<ReviewResponseDto>> GetAllAsync();
        Task<ReviewResponseDto> GetByIdAsync(int id);
        Task<List<ReviewResponseDto>> GetByCustomerIdAsync(int customerId);
    }
}
