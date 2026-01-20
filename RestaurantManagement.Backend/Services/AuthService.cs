using RestaurantManagement.Backend.Exceptions;
using RestaurantManagement.Backend.Helper;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.DataAccess.Repositories.Interfaces;
using RestaurantManagement.Dtos.Authentication;

namespace RestaurantManagement.Backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly JwtTokenGenerator _jwt;

        public AuthService(IUserRepository userRepo, JwtTokenGenerator jwt)
        {
            _userRepo = userRepo;
            _jwt = jwt;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
        {
            var user = await _userRepo.GetByEmailAsync(dto.Email.Trim().ToLower());
            if (user == null)
                throw new UnauthorizedException("Invalid email or password.");

            if (!user.IsActive)
                throw new UnauthorizedException("User is deactivated.");

            var result = PasswordHasher.Verify(dto.Password, user.Password);

            if (!result)
                throw new UnauthorizedException("Invalid email or password.");

            var token = _jwt.GenerateToken(user);

            return new AuthResponseDto
            {
                UserId = user.Id,
                FullName = user.Name,
                Role = user.Role.ToString(),
                Token = token
            };
        }
    }
}