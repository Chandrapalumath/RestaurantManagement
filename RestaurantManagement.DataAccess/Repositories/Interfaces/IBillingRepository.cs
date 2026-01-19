using RestaurantManagement.DataAccess.Models;

namespace RestaurantManagement.DataAccess.Repositories.Interfaces
{
    public interface IBillingRepository : IGenericRepository<Bill>
    {
        Task<Bill?> GetBillDetailsAsync(Guid billId);
        Task<List<Bill>> GetBillsByCustomerIdAsync(Guid customerId);
        Task<List<Bill>> GetAllBillsAsync();
    }
}
