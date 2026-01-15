using RestaurantManagement.DataAccess.Models;

namespace RestaurantManagement.DataAccess.Repositories.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order?> GetOrderWithItemsAsync(int orderId);
        Task<List<Order>> GetOrdersByCustomerIdAsync(int customerId);
        Task<List<Order>> GetOrdersForChefAsync(string? status);
        Task<List<Order>> GetCompletedUnbilledOrdersByCustomerAsync(int customerId);

    }
}
