using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.ViewModels.Order
{
    public class OrderListVm
    {
        public List<OrderLookupVm> Orders {  get; set; }
        public int Count { get; set; }
    }

}
