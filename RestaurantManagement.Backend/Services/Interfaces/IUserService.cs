using RestaurantManagement.Dtos.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Backend.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDto> CreateStaffUserAsync(CreateUserRequestDto dto);
        Task<List<UserResponseDto>> GetAllUsersAsync();
        Task<UserResponseDto> GetUserByIdAsync(int id);
        Task<string> UpdateUserAsync(int id, UserUpdateRequestDto dto);
    }
}
