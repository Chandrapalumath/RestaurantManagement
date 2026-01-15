using RestaurantManagement.Dtos.Menu;
using RestaurantManagement.Dtos.Orders;

namespace RestaurantManagement.Backend.Services.Interfaces
{
    public interface IMenuService
    {
        Task<List<MenuItemResponseDto>> GetAllAsync();
        Task<MenuItemResponseDto> GetByIdAsync(int id);
        Task<MenuItemResponseDto> CreateAsync(MenuItemCreateRequestDto dto);
        Task<MenuItemResponseDto> UpdateAsync(int id, MenuItemUpdateRequestDto dto);
        Task DeleteAsync(int id);
        Task<MenuItemResponseDto> UpdateAvailabilityAsync(int id, bool isAvailable);
    }
}
