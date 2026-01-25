using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.Models.Common.Enums;

namespace RestaurantManagement.DataAccess.Repositories.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order?> GetOrderWithItemsAsync(Guid orderId);
        Task<List<Order>> GetOrdersForChefAsync(OrderStatus? status);
        Task<List<Order>> GetNotBilledOrders(Guid TableId);
        Task<List<Order>> GetOrderWithTableIdAsync(Guid tableId);
    }
}
