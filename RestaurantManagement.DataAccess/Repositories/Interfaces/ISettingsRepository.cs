using RestaurantManagement.DataAccess.Models;

namespace RestaurantManagement.DataAccess.Repositories.Interfaces
{
    public interface ISettingsRepository : IGenericRepository<RestaurantSettings>
    {
        Task<RestaurantSettings?> GetSettingsAsync();
    }

}
