using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.ViewModels.Product
{
    public class ProductListVm
    {
        public List<ProductLookupVm> Products { get; set; }
        public int Count { get; set; }
    }

}
