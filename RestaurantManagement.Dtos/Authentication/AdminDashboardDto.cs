namespace RestaurantManagement.Dtos.Authentication
{
    public class AdminDashboardDto
    {
        public int TotalTables { get; set; }
        public int OccupiedTables { get; set; }
        public decimal TotalSales { get; set; }
        public decimal TaxPercent { get; set; }
        public decimal DiscountPercent { get; set; }
        public double AverageRating { get; set; }
        public List<TopWaiterDto> TopWaiters { get; set; } = new();
    }
}
