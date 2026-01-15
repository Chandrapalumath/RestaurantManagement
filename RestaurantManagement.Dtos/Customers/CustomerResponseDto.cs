namespace RestaurantManagement.Dtos.Customers
{
    public class CustomerResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
