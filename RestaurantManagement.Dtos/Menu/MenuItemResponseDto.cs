namespace RestaurantManagement.Dtos.Menu
{
    public class MenuItemResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public int Rating { get; set; }
        public Guid? CustomerId { get; set; }
    }
}
