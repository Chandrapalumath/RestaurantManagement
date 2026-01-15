//using RestaurantManagement.Backend.Exceptions;
//using RestaurantManagement.DataAccess.Models;
//using RestaurantManagement.DataAccess.Models.Enums;
//using RestaurantManagement.DataAccess.Repositories.Interfaces;

//namespace RestaurantManagement.Backend.Services
//{
//    public class AuthService : IAuthService
//    {
//        private readonly IUserRepository _userRepo;
//        private readonly PasswordHasher<User> _hasher;
//        private readonly JwtTokenGenerator _jwt;

//        public AuthService(IUserRepository userRepo, JwtTokenGenerator jwt)
//        {
//            _userRepo = userRepo;
//            _jwt = jwt;
//            _hasher = new PasswordHasher<User>();
//        }

//        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto)
//        {
//            var existing = await _userRepo.GetByEmailAsync(dto.Email);
//            if (existing != null)
//                throw new BadRequestException("Email already registered.");

//            if (!Enum.TryParse<UserRole>(dto.Role, true, out var role))
//                throw new BadRequestException("Invalid role.");

//            var user = new User
//            {
//                Email = dto.Email.Trim().ToLower(),
//                FullName = dto.FullName.Trim(),
//                MobileNumber = dto.MobileNumber.Trim(),
//                Role = role,
//                IsActive = true
//            };

//            user.PasswordHash = _hasher.HashPassword(user, dto.Password);

//            await _userRepo.AddAsync(user);
//            await _userRepo.SaveChangesAsync();

//            var token = _jwt.GenerateToken(user);

//            return new AuthResponseDto
//            {
//                UserId = user.Id,
//                FullName = user.FullName,
//                Role = user.Role.ToString(),
//                Token = token
//            };
//        }

//        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
//        {
//            var user = await _userRepo.GetByEmailAsync(dto.Email.Trim().ToLower());
//            if (user == null)
//                throw new UnauthorizedException("Invalid email or password.");

//            if (!user.IsActive)
//                throw new UnauthorizedException("User is deactivated.");

//            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
//            if (result == PasswordVerificationResult.Failed)
//                throw new UnauthorizedException("Invalid email or password.");

//            var token = _jwt.GenerateToken(user);

//            return new AuthResponseDto
//            {
//                UserId = user.Id,
//                FullName = user.FullName,
//                Role = user.Role.ToString(),
//                Token = token
//            };
//        }
//    }

//}