using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagement.DataAccess.Models
{
    public class Bill
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int GeneratedByWaiterId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountAmount { get; set; }

        public decimal TaxPercent { get; set; }
        public decimal TaxAmount { get; set; }

        public decimal GrandTotal { get; set; }

        public bool IsPaymentDone { get; set; } = false;

        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
        [ForeignKey(nameof(CustomerId))]
        public Customer? Customer { get; set; }
        [ForeignKey(nameof(OrderId))]
        public Order? Order { get; set; }
        [ForeignKey(nameof(GeneratedByWaiterId))]
        public User? GeneratedByWaiter { get; set; }
    }

}
