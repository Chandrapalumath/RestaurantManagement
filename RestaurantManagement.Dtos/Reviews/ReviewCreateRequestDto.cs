using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Dtos.Reviews
{
    public class ReviewCreateRequestDto
    {
        [Required]
        public Guid CustomerId { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 to 5.")]
        public int Rating { get; set; }

        [MaxLength(500)]
        public string? Comment { get; set; }
    }
}
