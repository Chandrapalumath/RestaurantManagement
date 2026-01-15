using Microsoft.EntityFrameworkCore;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Repositories.Interfaces;

namespace RestaurantManagement.DataAccess.Repositories
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(RestaurantDbContext context) : base(context) { }

        public async Task<List<Review>> GetByCustomerIdAsync(int customerId)
            => await _context.Reviews
                .Where(r => r.CustomerId == customerId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
    }

}
