using RestaurantManagement.Dtos.Customers;
using RestaurantManagement.Dtos.Pagination;

namespace RestaurantManagement.Backend.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerResponseDto> CreateAsync(CustomerCreateRequestDto dto);
        Task<CustomerResponseDto> GetByIdAsync(Guid id);
        Task<PagedResult<CustomerResponseDto>> GetAllAsync(int page, int pageSize, string? search);
        Task<List<CustomerResponseDto>> GetByMobileNumberAsync(string mobile);
    }
}
