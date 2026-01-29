using RestaurantManagement.Backend.Exceptions;
using RestaurantManagement.Backend.Helper;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.Backend.Utils;
using RestaurantManagement.DataAccess.Repositories.Interfaces;
using RestaurantManagement.Dtos.Authentication;

namespace RestaurantManagement.Backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IJwtTokenGenerator _jwt;

        public AuthService(IUserRepository userRepo, IJwtTokenGenerator jwt)
        {
            _userRepo = userRepo;
            _jwt = jwt;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
        {
            var user = await _userRepo.GetByEmailAsync(dto.Email.Trim().ToLower());
            if (user is null)
                throw new UnauthorizedException("Invalid email or password.");

            if (!user.IsActive)
                throw new UnauthorizedException("User is deactivated.");

            var result = PasswordHasher.Verify(dto.Password, user.Password);

            if (!result)
                throw new UnauthorizedException("Invalid password.");

            var token = _jwt.GenerateToken(user);

            return new AuthResponseDto
            {
                FullName = user.Name,
                Role = user.Role.ToString(),
                Token = token
            };
        }

        public async Task ChangePasswordAsync(Guid userId, ChangePasswordRequestDto dto)
        {
            var user = await _userRepo.GetByIdAsync(userId)
                       ?? throw new UnauthorizedException("User not found.");

            var isValid = PasswordHasher.Verify(dto.CurrentPassword, user.Password);
            if (!isValid)
                throw new BadRequestException("Current password is incorrect.");

            if (dto.CurrentPassword == dto.NewPassword)
                throw new BadRequestException("New password cannot be same as current password.");

            user.Password = PasswordHasher.Hash(dto.NewPassword);

            await _userRepo.SaveChangesAsync();
        }
    }
}