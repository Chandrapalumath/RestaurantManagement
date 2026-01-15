using RestaurantManagement.Dtos.Billing;

namespace RestaurantManagement.Backend.Services.Interfaces
{
    public interface IBillingService
    {
        Task<BillResponseDto> GenerateBillByOrderIdAsync(int orderId, int waiterId);
        Task MarkPaymentDoneAsync(int billId, int waiterId);
        Task<BillResponseDto> GetBillByIdAsync(int billId, int? waiterId, bool isAdmin);
        Task<List<BillResponseDto>> GetBillsByCustomerIdAsync(int customerId, int? waiterId, bool isAdmin);
        Task<List<BillResponseDto>> GetAllBillsAsync();
    }
}
