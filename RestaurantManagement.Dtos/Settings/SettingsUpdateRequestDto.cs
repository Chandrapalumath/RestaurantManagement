using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Dtos.Settings
{
    public class SettingsUpdateRequestDto
    {
        [Range(0, 100, ErrorMessage = "Tax percent must be between 0 and 100.")]
        public decimal? TaxPercent { get; set; }

        [Range(0, 100, ErrorMessage = "Discount percent must be between 0 and 100.")]
        public decimal? DiscountPercent { get; set; }
    }
}
