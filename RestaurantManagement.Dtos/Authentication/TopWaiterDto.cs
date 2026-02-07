namespace RestaurantManagement.Dtos.Authentication
{
    public class TopWaiterDto
    {
        public Guid WaiterId { get; set; }
        public string WaiterName { get; set; } = string.Empty;
        public int OrdersServed { get; set; }
    }
}
