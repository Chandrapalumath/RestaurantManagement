using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Dtos.Billing
{
    public class BillResponseDto
    {
        public int BillId { get; set; }
        public int CustomerId { get; set; }
        public int OrderId { get; set; }

        public decimal SubTotal { get; set; }

        public decimal DiscountPercent { get; set; }
        public decimal DiscountAmount { get; set; }

        public decimal TaxPercent { get; set; }
        public decimal TaxAmount { get; set; }

        public decimal GrandTotal { get; set; }

        public bool IsPaymentDone { get; set; }
        public DateTime GeneratedAt { get; set; }
    }
}
