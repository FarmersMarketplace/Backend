using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.ViewModels.Order
{
    public class OrderListVm
    {
        public List<ProducerOrderLookupVm> Orders {  get; set; }
        public int Count { get; set; }
    }

}
