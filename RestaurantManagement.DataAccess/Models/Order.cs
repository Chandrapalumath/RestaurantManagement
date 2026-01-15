using RestaurantManagement.DataAccess.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagement.DataAccess.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public int WaiterId { get; set; }
        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        [ForeignKey(nameof(CustomerId))]
        public Customer? Customer { get; set; }
        [ForeignKey(nameof(WaiterId))]
        public User? Waiter { get; set; }

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

        public Bill Bill { get; set; } 
    }

}
