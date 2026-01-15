using RestaurantManagement.DataAccess.Models;

namespace RestaurantManagement.DataAccess.Repositories.Interfaces
{
    public interface IBillingRepository : IGenericRepository<Bill>
    {
        Task<Bill?> GetBillDetailsAsync(int billId);
        Task<List<Bill>> GetBillsByCustomerIdAsync(int customerId);
        Task<List<Bill>> GetAllBillsAsync();
    }

}
