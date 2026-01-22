using RestaurantManagement.Models.Common.Enums;

namespace RestaurantManagement.Dtos.Orders
{
    public class OrderResponseDto
    {
        public Guid OrderId { get; set; }
        public Guid TableId { get; set; }
        public Guid WaiterId { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<OrderItemResponseDto> Items { get; set; } = new();
    }
}
