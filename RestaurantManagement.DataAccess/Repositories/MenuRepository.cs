using Microsoft.EntityFrameworkCore;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Repositories.Interfaces;

namespace RestaurantManagement.DataAccess.Repositories
{
    public class MenuRepository : GenericRepository<MenuItem>, IMenuRepository
    {
        public MenuRepository(RestaurantDbContext context) : base(context) { }

        public async Task<MenuItem?> GetByNameAsync(string name)
            => await _context.MenuItems.FirstOrDefaultAsync(x => x.Name == name);
    }
}
