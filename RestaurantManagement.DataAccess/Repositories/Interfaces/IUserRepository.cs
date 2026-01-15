using RestaurantManagement.DataAccess.Models;

namespace RestaurantManagement.DataAccess.Repositories.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
    }

}
