using RestaurantManagement.DataAccess.Models;

namespace RestaurantManagement.Backend.Utils
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}