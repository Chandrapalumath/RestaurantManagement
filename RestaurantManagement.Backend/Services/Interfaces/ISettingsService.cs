using RestaurantManagement.Dtos.Settings;

namespace RestaurantManagement.Backend.Services.Interfaces
{
    public interface ISettingsService
    {
        Task<SettingsResponseDto> GetSettingsAsync();
        Task<SettingsResponseDto> UpdateSettingsAsync(SettingsUpdateRequestDto dto);
    }

}
