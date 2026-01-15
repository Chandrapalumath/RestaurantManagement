using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Dtos.Users
{
    public class CreateUserRequestDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(3), MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be 10 digits.")]
        public string MobileNumber { get; set; } = string.Empty;

        [Required, MinLength(6), MaxLength(50)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^(Chef|Waiter)$", ErrorMessage = "Role must be Chef or Waiter.")]
        public string Role { get; set; } = "Waiter";
    }
}
