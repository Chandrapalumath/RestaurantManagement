using RestaurantManagement.Dtos.Settings;

namespace RestaurantManagement.Backend.Services.Interfaces
{
    public interface ISettingsService
    {
        Task<SettingsResponseDto> GetSettingsAsync();
        Task UpdateSettingsAsync(SettingsUpdateRequestDto dto);
    }

}
