using RestaurantManagement.Dtos.Billing;
using RestaurantManagement.Dtos.Menu;

namespace RestaurantManagement.Backend.Services.Interfaces
{
    public interface IBillingService
    {
        Task UpdateBill(Guid billId, Guid waiterId, BillUpdateRequestDto dto);
        Task<BillResponseDto> GetBillByIdAsync(Guid billId, Guid? waiterId, bool isAdmin);
        Task<List<BillResponseDto>> GetAllBillsAsync();
        Task<BillResponseDto> GenerateBillAsync(Guid waiterId, BillGenerateRequestDto dto);
        Task<List<MenuItemResponseDto>> GetMenuItemByBillIdAsync(Guid billId);
    }
}

