namespace RestaurantManagement.Dtos.Settings
{
    public class SettingsResponseDto
    {
        public decimal TaxPercent { get; set; }
        public decimal DiscountPercent { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
