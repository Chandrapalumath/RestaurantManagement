using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Models.Enums;
using RestaurantManagement.DataAccess.Repositories.Interfaces;
using RestaurantManagement.Dtos.Users;

namespace RestaurantManagement.Backend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;

        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<UserResponseDto> CreateStaffUserAsync(CreateUserRequestDto dto)
        {
            var existing = await _userRepo.GetByEmailAsync(dto.Email.Trim().ToLower());
            if (existing != null)
                throw new Exception("Email already exists.");

            if (!Enum.TryParse<UserRole>(dto.Role, true, out var role))
                throw new Exception("Invalid role.");

            if (role == UserRole.Admin)
                throw new Exception("Admin cannot be created here.");

            var user = new User
            {
                Email = dto.Email.Trim().ToLower(),
                Name = dto.FullName.Trim(),
                MobileNumber = dto.MobileNumber.Trim(),
                Role = role,
                IsActive = true,
                Password = dto.Password,
            };

            await _userRepo.AddAsync(user);
            await _userRepo.SaveChangesAsync();

            return new UserResponseDto
            {
                Id = user.Id,
                FullName = user.Name,
                MobileNumber = user.MobileNumber,
                Email = user.Email,
                Role = user.Role.ToString(),
                IsActive = user.IsActive
            };
        }


        public async Task<List<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _userRepo.GetAllAsync();
            return users.Select(u => new UserResponseDto
            {
                Id = u.Id,
                FullName = u.Name,
                MobileNumber = u.MobileNumber,
                Email = u.Email,
                Role = u.Role.ToString(),
                IsActive = u.IsActive
            }).ToList();
        }

        public async Task<UserResponseDto> GetUserByIdAsync(int id)
        {
            var user = await _userRepo.GetByIdAsync(id)
                       ?? throw new Exception("User not found.");

            return new UserResponseDto
            {
                Id = user.Id,
                FullName = user.Name,
                MobileNumber = user.MobileNumber,
                Email = user.Email,
                Role = user.Role.ToString(),
                IsActive = user.IsActive
            };
        }

        public async Task<string> UpdateUserAsync(int id, UserUpdateRequestDto dto)
        {
            var user = await _userRepo.GetByIdAsync(id)
                       ?? throw new Exception("User not found.");

            if (!string.IsNullOrWhiteSpace(dto.FullName))
                user.Name = dto.FullName.Trim();

            if (!string.IsNullOrWhiteSpace(dto.MobileNumber))
                user.MobileNumber = dto.MobileNumber.Trim();

            if (!string.IsNullOrWhiteSpace(dto.Email))
                user.Email = dto.Email.Trim().ToLower();

            if (dto.IsActive.HasValue)
                user.IsActive = dto.IsActive.Value;

            if (!Enum.TryParse<UserRole>(dto.Role, true, out var role))
                throw new Exception("Invalid role.");

            if (role == UserRole.Admin)
                throw new Exception("Admin cannot be created here.");

            _userRepo.Update(user);
            await _userRepo.SaveChangesAsync();

            return "Updated Successfully";
        }
    }
}
