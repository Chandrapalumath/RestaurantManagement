using RestaurantManagement.DataAccess.Models;

namespace RestaurantManagement.DataAccess.Repositories.Interfaces
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<Customer?> GetByMobileAsync(string mobileNumber);
    }
}
