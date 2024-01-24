using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Domain
{
    public enum OrderStatus
    {
        New,
        Processing,
        Collected,
        InDelivery,
        Completed
    }

}
