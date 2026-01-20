using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Dtos.Menu
{
    public class UpdateMenuItemRating
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [Range(0,5)]
        public int? Rating { get; set; }
    }

}
