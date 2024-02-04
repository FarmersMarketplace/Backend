using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.ViewModels.Product
{
    public class AllProductsVm
    {
        public List<ProductLookupVm> Products {  get; set; }
        public FilterData FilterData { get; set; }
    }

}
