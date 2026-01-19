namespace RestaurantManagement.Dtos.Orders
{
    public class OrderResponseDto
    {
        public Guid OrderId { get; set; }
        public Guid TableId { get; set; }
        public Guid WaiterId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public List<OrderItemResponseDto> Items { get; set; } = new();
    }
}
