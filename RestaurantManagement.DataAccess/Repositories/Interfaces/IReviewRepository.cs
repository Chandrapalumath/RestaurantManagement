using RestaurantManagement.DataAccess.Models;

namespace RestaurantManagement.DataAccess.Repositories.Interfaces
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task<List<Review>> GetByCustomerIdAsync(int customerId);
    }

}
