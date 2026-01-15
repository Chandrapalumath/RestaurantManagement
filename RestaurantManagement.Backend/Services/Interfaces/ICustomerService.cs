using RestaurantManagement.Dtos.Customers;

namespace RestaurantManagement.Backend.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerResponseDto> CreateAsync(CustomerCreateRequestDto dto);
        Task<CustomerResponseDto> GetByIdAsync(int id);
        Task<CustomerResponseDto?> GetByMobileAsync(string mobileNumber);
        Task<List<CustomerResponseDto>> GetAllAsync();
    }
}
