using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.Models.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagement.DataAccess.Models
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid TableId { get; set; }
        [Required]
        public Guid WaiterId { get; set; }
        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsBilled { get; set; } = false;
        public Guid? BillingId { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [ForeignKey(nameof(TableId))]
        public Table? Table { get; set; }
        [ForeignKey(nameof(WaiterId))]
        public User? Waiter { get; set; }

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

        [ForeignKey(nameof(BillingId))]
        public Bill? Bill { get; set; }
    }

}
