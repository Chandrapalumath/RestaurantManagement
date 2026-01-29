using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Dtos.Authentication
{
    public class ChangePasswordRequestDto
    {
        [Required, MinLength(12), MaxLength(20)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).*$",
            ErrorMessage = "Password must contain uppercase, lowercase, number and special character."
        )]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required, MinLength(12), MaxLength(20)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).*$",
            ErrorMessage = "Password must contain uppercase, lowercase, number and special character."
        )]
        public string NewPassword { get; set; } = string.Empty;
    }
}
