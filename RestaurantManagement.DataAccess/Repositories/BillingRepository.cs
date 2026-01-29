using Microsoft.EntityFrameworkCore;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Repositories.Interfaces;

namespace RestaurantManagement.DataAccess.Repositories
{
    public class BillingRepository : GenericRepository<Bill>, IBillingRepository
    {
        public BillingRepository(RestaurantDbContext context) : base(context) { }

        public async Task<Bill?> GetBillDetailsAsync(Guid billId)
            => await _context.Bills
            .AsNoTracking()
        .Include(b => b.Customer)
        .Include(b => b.GeneratedByWaiter)
        .Include(b => b.Orders)                
            .ThenInclude(o => o.Items)          
                .ThenInclude(i => i.MenuItem)    
        .FirstOrDefaultAsync(b => b.Id == billId);

        public async Task<List<Bill>> GetBillsByCustomerIdAsync(Guid customerId)
            => await _context.Bills
            .AsNoTracking()
                .Where(b => b.CustomerId == customerId)
                .OrderByDescending(b => b.GeneratedAt)
                .ToListAsync();

        public async Task<List<Bill>> GetAllBillsAsync()
            => await _context.Bills.AsNoTracking()
                .OrderByDescending(b => b.GeneratedAt)
                .ToListAsync();
    }
}
