using Microsoft.EntityFrameworkCore;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Repositories.Interfaces;

namespace RestaurantManagement.DataAccess.Repositories
{
    public class SettingsRepository : GenericRepository<RestaurantSettings>, ISettingsRepository
    {
        public SettingsRepository(RestaurantDbContext context) : base(context) { }

        public async Task<RestaurantSettings?> GetSettingsAsync()
            => await _context.RestaurantSettings.FirstOrDefaultAsync(x => x.Id == 1);
    }
}
