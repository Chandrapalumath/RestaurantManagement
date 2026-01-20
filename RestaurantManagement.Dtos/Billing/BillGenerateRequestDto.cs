using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Dtos.Billing
{
    public class BillGenerateRequestDto
    {
        [Required]
        public Guid CustomerId { get; set; }
        [Required]
        public List<Guid> OrdersId { get; set; } = new List<Guid>();
    }
}
