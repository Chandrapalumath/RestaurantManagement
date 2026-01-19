using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using RestaurantManagement.Models.Common.Enums;

namespace RestaurantManagement.Dtos.Authentication
{
    public class RegisterRequestDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(3), MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be 10 digits.")]
        public string MobileNumber { get; set; } = string.Empty;

        [Required, MinLength(12), MaxLength(20)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).*$",
            ErrorMessage = "Password must contain uppercase, lowercase, number and special character."
        )]
        public string Password { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; }
    }
}
