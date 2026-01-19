using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.DataAccess.Models
{
    public class Table
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(50)]
        public string TableName { get; set; } = string.Empty;   // like T1, T2, VIP-1

        [Range(1, 50)]
        public int Size { get; set; }  // number of seats (future scope)

        public bool IsOccupied { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}

