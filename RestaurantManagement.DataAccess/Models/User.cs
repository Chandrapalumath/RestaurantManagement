using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.DataAccess.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class User // Admin, Waiter, Chef
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MinLength(3),MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must contain 10 digits")]
        public string MobileNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public UserRole Role { get; set; } 
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Order> OrdersTaken { get; set; } = new List<Order>();
        public ICollection<Bill> BillsGenerated { get; set; } = new List<Bill>();
    }
}
