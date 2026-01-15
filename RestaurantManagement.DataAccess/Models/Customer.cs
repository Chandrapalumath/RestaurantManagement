using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.DataAccess.Models
{
    [Index(nameof(MobileNumber), IsUnique = true)]
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MaxLength(10)]
        public string MobileNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Bill> Bills { get; set; } = new List<Bill>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }

}
