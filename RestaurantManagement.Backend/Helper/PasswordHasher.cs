using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Backend.Helper
{
    public static class PasswordHasher
    {
        public static string Hash(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password.Trim());
        }

        public static bool Verify(string password, string hash)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password.Trim(), hash);
        }
    }
}
