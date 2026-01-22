using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.Dtos.Orders;
using RestaurantManagement.Models.Common.Enums;

namespace RestaurantManagement.Backend.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponseDto> CreateOrderAsync(OrderCreateRequestDto dto, Guid waiterId);
        Task<OrderResponseDto> GetByIdAsync(Guid id);
        Task<List<OrderResponseDto>> GetOrdersAsync(OrderStatus status);
        Task<List<OrderResponseDto>> GetOrdersByTableIdAsync(Guid id);
        Task UpdateOrderAsync(Guid orderId, OrderUpdateRequestDto dto);
    }
}
