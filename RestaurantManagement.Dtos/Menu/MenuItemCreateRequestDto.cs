using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Dtos.Menu
{
    public class MenuItemCreateRequestDto
    {
        [Required, MinLength(2), MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(1, 999999, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; } = true;
    }

}
