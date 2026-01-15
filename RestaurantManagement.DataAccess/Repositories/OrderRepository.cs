using Microsoft.EntityFrameworkCore;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Models.Enums;
using RestaurantManagement.DataAccess.Repositories.Interfaces;

namespace RestaurantManagement.DataAccess.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(RestaurantDbContext context) : base(context) { }

        public async Task<Order?> GetOrderWithItemsAsync(int orderId)
            => await _context.Orders
                .Include(o => o.Items)
                    .ThenInclude(i => i.MenuItem)
                .FirstOrDefaultAsync(o => o.Id == orderId);

        public async Task<List<Order>> OrdersByDayAsync(DateTime day)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .Where(o => o.CreatedAt.Date == day)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Order>> GetOrdersByCustomerIdAsync(int customerId)
            => await _context.Orders
                .Include(o => o.Items)
                .Where(o => o.CustomerId == customerId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

        public async Task<List<Order>> GetOrdersForChefAsync(string? status)
        {
            IQueryable<Order> query = _context.Orders
                .Include(o => o.Items)
                    .ThenInclude(i => i.MenuItem);

            if (!string.IsNullOrWhiteSpace(status) &&
                Enum.TryParse<OrderStatus>(status, true, out var parsedStatus))
            {
                query = query.Where(o => o.Status == parsedStatus);
            }
            else
            {
                query = query.Where(o => o.Status == OrderStatus.Pending || o.Status == OrderStatus.Preparing);
            }

            return await query.OrderByDescending(o => o.CreatedAt).ToListAsync();
        }

        public async Task<Order?> GetLatestCompletedOrderByCustomerAsync(int customerId)
            => await _context.Orders
                .Include(o => o.Items)
                    .ThenInclude(i => i.MenuItem)
                .Where(o => o.CustomerId == customerId && o.Status == OrderStatus.Completed)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync();
    }
}
