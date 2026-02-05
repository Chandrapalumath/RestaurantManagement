namespace RestaurantManagement.Dtos.Billing
{
    public class BillResponseDto
    {
        public Guid BillId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid WaiterId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxPercent { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public bool IsPaymentDone { get; set; }
        public DateTime GeneratedAt { get; set; }
    }
}
