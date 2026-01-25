using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagement.DataAccess.Models
{
    public class Review
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid CustomerId { get; set; }
        [Range(1,5)]
        public int Rating { get; set; }
        [MaxLength(500)]
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [ForeignKey(nameof(CustomerId))]
        public Customer? Customer { get; set; } 
    }
}
