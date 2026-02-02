using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagement.DataAccess.Models
{
    public class Table
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(50)]
        public string TableName { get; set; }

        [Range(1, 50)]
        public int Capacity { get; set; } 

        public bool IsOccupied { get; set; } 

        public DateTime CreatedAt { get; set; } 
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public Guid? OccupiedByWaiterId { get; set; }

        [ForeignKey(nameof(OccupiedByWaiterId))]
        public User? OccupiedByWaiter { get; set; }
    }
}

