using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagement.DataAccess.Models
{
    public class RestaurantSettings
    {
        [Key]
        public Guid Id { get; set; }
        [Range(0,100)]
        public decimal TaxPercent { get; set; } = 0;
        [Range(0,100)]
        public decimal DiscountPercent { get; set; } = 0;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public Guid UpdatedByAdminId { get; set; }
        [ForeignKey(nameof(UpdatedByAdminId))]
        public User? UpdatedByAdmin { get; set; }
    }

}
