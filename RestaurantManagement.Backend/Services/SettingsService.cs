using RestaurantManagement.Backend.Exceptions;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Repositories.Interfaces;
using RestaurantManagement.Dtos.Settings;

namespace RestaurantManagement.Backend.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly ISettingsRepository _settingsRepo;
        private readonly IUserRepository _userRepo;

        public SettingsService(ISettingsRepository settingsRepo, IUserRepository userRepo)
        {
            _settingsRepo = settingsRepo;
            _userRepo = userRepo;
        }

        public async Task<SettingsResponseDto> GetSettingsAsync()
        {
            var settings = await _settingsRepo.GetSettingsAsync();
            if (settings == null)
                throw new NotFoundException("Settings not found.");

            return new SettingsResponseDto
            {
                TaxPercent = settings.TaxPercent,
                DiscountPercent = settings.DiscountPercent,
                UpdatedAt = settings.UpdatedAt
            };
        }

        public async Task UpdateSettingsAsync(SettingsUpdateRequestDto dto)
        {
            var settings = await _settingsRepo.GetSettingsAsync();

            if (settings == null)
            {
                settings = new RestaurantSettings { Id = Guid.NewGuid() };
                await _settingsRepo.AddAsync(settings);
            }

            if(dto.TaxPercent.HasValue)
                settings.TaxPercent = dto.TaxPercent.Value;
            if (dto.DiscountPercent.HasValue)
                settings.DiscountPercent = dto.DiscountPercent.Value;
            settings.UpdatedAt = DateTime.UtcNow;

            await _settingsRepo.SaveChangesAsync();
        }
    }
}