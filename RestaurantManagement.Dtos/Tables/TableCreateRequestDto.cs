using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Dtos.Tables
{
    public class TableCreateRequestDto
    {
        [Required, MaxLength(50)]
        public string TableName { get; set; } = string.Empty;

        [Range(1, 50)]
        public int Size { get; set; }
    }
}
