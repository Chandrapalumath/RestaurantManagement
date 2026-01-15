using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.DataAccess.Models.Enums
{
    public enum OrderStatus
    {
        Pending = 1,
        Preparing = 2,
        Completed = 3,
        Served = 4
    }
}
