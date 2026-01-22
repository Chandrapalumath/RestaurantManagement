using Microsoft.EntityFrameworkCore;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Repositories.Interfaces;
using RestaurantManagement.Models.Common.Enums;

namespace RestaurantManagement.DataAccess.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(RestaurantDbContext context) : base(context) { }

        public async Task<Order?> GetOrderWithItemsAsync(Guid orderId)
            => await _context.Orders
                .Include(o => o.Items)
                    .ThenInclude(i => i.MenuItem)
                .FirstOrDefaultAsync(o => o.Id == orderId);

        public async Task<List<Order>> GetOrderWithTableIdAsync(Guid tableId)
        => await _context.Orders
            .Include(o => o.Items)
                .ThenInclude(i => i.MenuItem)
            .Where(o => o.TableId == tableId && !o.IsBilled)
            .ToListAsync();

        public async Task<List<Order>> GetNotBilledOrders(Guid TableId)
        {
            return await _context.Orders.Where(o => o.TableId == TableId && o.IsBilled == false).ToListAsync();
        }

        public async Task<List<Order>> GetOrdersForChefAsync(OrderStatus status)
        {
            IQueryable<Order> query = _context.Orders
                .Include(o => o.Items)
                    .ThenInclude(i => i.MenuItem);

            if (status!=null)
            {
                query = query.Where(o => o.Status == status);
            }
            else
            {
                query = query.Where(o => o.Status == OrderStatus.Pending || o.Status == OrderStatus.Preparing);
            }

            return await query.OrderByDescending(o => o.CreatedAt).ToListAsync();
        }
    }
}
