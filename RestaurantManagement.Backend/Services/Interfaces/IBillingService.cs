using RestaurantManagement.Dtos.Billing;

namespace RestaurantManagement.Backend.Services.Interfaces
{
    public interface IBillingService
    {
        Task UpdateBill(int billId, int waiterId, BillUpdateRequestDto dto);
        Task<BillResponseDto> GetBillByIdAsync(int billId, int? waiterId, bool isAdmin);
        Task<List<BillResponseDto>> GetBillsByCustomerIdAsync(int customerId, int? waiterId, bool isAdmin);
        Task<List<BillResponseDto>> GetAllBillsAsync();
        Task<BillResponseDto> GenerateBillAsync(int customerId, int waiterId);
    }
}
