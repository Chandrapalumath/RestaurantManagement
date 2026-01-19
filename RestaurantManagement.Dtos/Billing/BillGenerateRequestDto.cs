using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Dtos.Billing
{
    public class BillGenerateRequestDto
    {
        [Required, MinLength(2), MaxLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        [Required, RegularExpression(@"^\d{10}$")]
        public string MobileNumber { get; set; } = string.Empty;
    }
}
