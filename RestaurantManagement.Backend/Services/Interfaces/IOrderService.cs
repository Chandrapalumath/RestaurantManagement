using RestaurantManagement.Dtos.Orders;

namespace RestaurantManagement.Backend.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponseDto> CreateOrderAsync(OrderCreateRequestDto dto, int waiterId);
        Task<OrderResponseDto> GetByIdAsync(int id);
        Task<List<OrderResponseDto>> GetOrdersByCustomerIdAsync(int customerId);
        //Task<List<OrderResponseDto>> OrdersByDayAsync(DateTime day);
    }
}
