using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Dtos.Orders
{
    public class OrderCreateRequestDto
    {
        [Required]
        public int CustomerId { get; set; }

        [Required, MinLength(1, ErrorMessage = "Order must contain at least 1 item.")]
        public List<OrderItemCreateDto> Items { get; set; } = new();
    }
}
