using RestaurantManagement.Dtos.Orders;

namespace RestaurantManagement.Backend.Services.Interfaces
{
    public interface IChefService
    {
        Task<List<OrderResponseDto>> GetOrdersAsync(string? status);
        Task<OrderResponseDto> GetOrderDetailsAsync(int orderId);
        Task<OrderResponseDto> UpdateOrderStatusAsync(int orderId, OrderStatusUpdateRequestDto dto);
    }
}
