using RestaurantManagement.Backend.Exceptions;
using RestaurantManagement.Backend.Helper;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Repositories.Interfaces;
using RestaurantManagement.Dtos.Users;
using RestaurantManagement.Models.Common.Enums;

namespace RestaurantManagement.Backend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;

        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<UserResponseDto> CreateUserAsync(CreateUserRequestDto dto)
        {
            var exists = await _userRepo.GetByEmailAsync(dto.Email.Trim().ToLower());
            if (exists is not null)
                throw new ConflictException("Email already exists.");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = dto.Email.Trim().ToLower(),
                Name = dto.FullName.Trim(),
                MobileNumber = dto.MobileNumber.Trim(),
                Role = dto.Role,
                IsActive = true,
                Password = PasswordHasher.Hash(dto.Password)
            };

            await _userRepo.AddAsync(user);
            await _userRepo.SaveChangesAsync();

            return new UserResponseDto
            {
                Id = user.Id,
                FullName = user.Name,
                MobileNumber = user.MobileNumber,
                Email = user.Email,
                Role = user.Role,
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
                Role = u.Role,
                IsActive = u.IsActive
            }).ToList();
        }

        public async Task<UserResponseDto> GetUserByIdAsync(Guid id, bool isAdmin, Guid? userId)
        {
            var user = await _userRepo.GetByIdAsync(id)
                       ?? throw new NotFoundException("User not found or Invalid ID.");

            if (!isAdmin)
            {
                if (!userId.HasValue || userId.Value != id)
                    throw new ForbiddenException("You are not allowed to view this user.");
            }

            return new UserResponseDto
            {
                Id = user.Id,
                FullName = user.Name,
                MobileNumber = user.MobileNumber,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive
            };
        }

        public async Task UpdateUserAsync(Guid id, UserUpdateRequestDto dto)
        {
            var user = await _userRepo.GetByIdAsync(id)
                       ?? throw new NotFoundException("User not found.");

            if (!string.IsNullOrWhiteSpace(dto.FullName))
                user.Name = dto.FullName.Trim();

            if (!string.IsNullOrWhiteSpace(dto.MobileNumber))
                user.MobileNumber = dto.MobileNumber.Trim();

            if (!string.IsNullOrWhiteSpace(dto.Email))
                user.Email = dto.Email.Trim().ToLower();

            if (dto.IsActive.HasValue)
                user.IsActive = dto.IsActive.Value;
            

            if (dto.Role == UserRole.Admin)
                throw new BadRequestException("Admin cannot be created here.");

            _userRepo.Update(user);
            await _userRepo.SaveChangesAsync();

        }
    }
}
