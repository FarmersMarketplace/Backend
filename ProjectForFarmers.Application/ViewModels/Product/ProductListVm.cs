using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.ViewModels.Product
{
    public class ProductListVm
    {
        public List<ProducerProductLookupVm> Products { get; set; }
        public int Count { get; set; }
    }

}
