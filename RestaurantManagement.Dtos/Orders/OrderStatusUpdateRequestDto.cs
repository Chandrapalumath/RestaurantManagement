using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Dtos.Orders
{
    public class OrderUpdateRequestDto
    {
        [RegularExpression("^(Pending|Preparing|Completed|Served)$",
            ErrorMessage = "Status must be Pending, Preparing, Completed or Served.")]
        public string? Status { get; set; } = "Pending";
    }
}
