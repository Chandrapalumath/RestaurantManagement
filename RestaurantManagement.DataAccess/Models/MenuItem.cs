using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.DataAccess.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class MenuItem
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MinLength(3),MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Range(0,999999)]
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; } = true;
        [Range(1,5)]
        public int? Rating { get; set; }
        public int? TotalReviews { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }

}
