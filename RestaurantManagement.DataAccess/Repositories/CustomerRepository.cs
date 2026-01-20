using Microsoft.EntityFrameworkCore;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Repositories.Interfaces;

namespace RestaurantManagement.DataAccess.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(RestaurantDbContext context) : base(context) { }

        public async Task<List<Customer>?> GetByMobileAsync(string mobileNumber)
            => await _context.Customers.Where(x => x.MobileNumber.StartsWith(mobileNumber)).ToListAsync();
    }

}
