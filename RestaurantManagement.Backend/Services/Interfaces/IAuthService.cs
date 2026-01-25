using RestaurantManagement.Dtos.Authentication;

namespace RestaurantManagement.Backend.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginRequestDto dto);
        Task ChangePasswordAsync(Guid userId, ChangePasswordRequestDto dto);

    }
}