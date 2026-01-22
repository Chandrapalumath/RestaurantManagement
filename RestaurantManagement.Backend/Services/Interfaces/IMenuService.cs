using RestaurantManagement.Dtos.Menu;
using RestaurantManagement.Dtos.Orders;

namespace RestaurantManagement.Backend.Services.Interfaces
{
    public interface IMenuService
    {
        Task<List<MenuItemResponseDto>> GetAllAsync();
        Task<MenuItemResponseDto> GetByIdAsync(Guid id);
        Task<MenuItemResponseDto> CreateAsync(MenuItemCreateRequestDto dto);
        Task UpdateAsync(Guid id, MenuItemUpdateRequestDto dto);
        Task DeleteAsync(Guid id);
        Task UpdateRatingAsync(List<UpdateMenuItemRating> dto);
    }
}
