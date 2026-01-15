namespace RestaurantManagement.Dtos.Orders
{
    public class OrderResponseDto
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int WaiterId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public List<OrderItemResponseDto> Items { get; set; } = new();
    }
    public class OrderItemResponseDto
    {
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal { get; set; }
    }
}
