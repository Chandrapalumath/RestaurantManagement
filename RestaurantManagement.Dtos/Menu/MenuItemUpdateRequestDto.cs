using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Dtos.Menu
{
    public class MenuItemUpdateRequestDto
    {
        [Required, MinLength(2), MaxLength(50)]
        public string? Name { get; set; } = string.Empty;
        [Range(1, 999999, ErrorMessage = "Price must be greater than 0.")]
        public decimal? Price { get; set; }
        public bool? IsAvailable { get; set; } = true;
    }
}