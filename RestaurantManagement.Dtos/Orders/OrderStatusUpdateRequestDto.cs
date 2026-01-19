using RestaurantManagement.Models.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Dtos.Orders
{
    public class OrderUpdateRequestDto
    {
        public OrderStatus? Status { get; set; }
    }
}
