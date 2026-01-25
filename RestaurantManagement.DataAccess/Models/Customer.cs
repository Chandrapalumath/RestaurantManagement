using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.DataAccess.Models
{
    [Index(nameof(MobileNumber), IsUnique = true)]
    public class Customer
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MinLength(3),MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MinLength(1), MaxLength(10)]
        public string MobileNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Bill> Bills { get; set; } = new List<Bill>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
