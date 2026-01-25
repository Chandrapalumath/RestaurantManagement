using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.DataAccess.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class User // Admin, Waiter, Chef
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MinLength(100),MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must contain 10 to 15 digits")]
        public string MobileNumber { get; set; } = string.Empty;
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public UserRole Role { get; set; } 
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Order> OrdersTaken { get; set; } = new List<Order>();
        public ICollection<Bill> BillsGenerated { get; set; } = new List<Bill>();
    }
}
