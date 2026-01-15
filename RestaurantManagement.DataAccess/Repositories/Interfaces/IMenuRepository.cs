using RestaurantManagement.DataAccess.Models;

namespace RestaurantManagement.DataAccess.Repositories.Interfaces
{
    public interface IMenuRepository : IGenericRepository<MenuItem>
    {
        Task<MenuItem?> GetByNameAsync(string name);
    }
}
