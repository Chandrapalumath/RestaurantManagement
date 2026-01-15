using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Dtos.Menu
{
    public class MenuItemUpdateRequestDto
    {
        public string? Name { get; set; } = string.Empty;
        public decimal? Price { get; set; }

        public bool? IsAvailable { get; set; } = true;
    }
}
