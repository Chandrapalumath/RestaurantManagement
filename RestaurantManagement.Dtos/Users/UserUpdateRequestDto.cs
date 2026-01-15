using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Dtos.Users
{
    public class UserUpdateRequestDto
    {
        [MaxLength(100)]
        public string? FullName { get; set; }

        [MaxLength(10)]
        public string? MobileNumber { get; set; }

        [EmailAddress]
        [MaxLength(150)]
        public string? Email { get; set; }

        public bool? IsActive { get; set; }

        [Required]
        [RegularExpression("^(Chef|Waiter)$", ErrorMessage = "Role must be Chef or Waiter.")]
        public string Role { get; set; } = "Waiter";
    }
}