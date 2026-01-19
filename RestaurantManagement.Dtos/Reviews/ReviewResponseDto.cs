namespace RestaurantManagement.Dtos.Reviews
{
    public class ReviewResponseDto
    {
        public Guid ReviewId { get; set; }
        public Guid CustomerId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
