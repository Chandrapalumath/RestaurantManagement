namespace RestaurantManagement.Dtos.Reviews
{
    public class ReviewResponseDto
    {
        public int ReviewId { get; set; }
        public int CustomerId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
