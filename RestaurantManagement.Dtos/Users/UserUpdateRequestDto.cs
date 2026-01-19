using RestaurantManagement.Models.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Dtos.Users
{
    public class UserUpdateRequestDto
    {
        [MinLength(3), MaxLength(50)]
        public string? FullName { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be 10 digits.")]
        public string? MobileNumber { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public bool? IsActive { get; set; }

        [Required]
        public UserRole Role { get; set; } 
    }
}