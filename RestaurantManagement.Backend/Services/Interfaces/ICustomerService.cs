using RestaurantManagement.Dtos.Customers;

namespace RestaurantManagement.Backend.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerResponseDto> CreateAsync(CustomerCreateRequestDto dto, Guid TableId);
        Task<CustomerResponseDto> GetByIdAsync(Guid id);
        Task<List<CustomerResponseDto>> GetAllAsync();
    }
}
