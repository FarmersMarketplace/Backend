using FarmersMarketplace.Application.ViewModels.Category;
using FarmersMarketplace.Application.ViewModels.Subcategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.ViewModels.Product
{
    public class ProducerProductFilterData
    {
        public List<string> UnitsOfMeasurement { get; set; }
        public List<SubcategoryVm> Subcategories { get; set; }

        public ProducerProductFilterData(List<string> unitsOfMeasurement)
        {
            UnitsOfMeasurement = unitsOfMeasurement;
        }

        public ProducerProductFilterData()
        {
            UnitsOfMeasurement = new List<string>();
        }
    }

}
