using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.ViewModels.Product
{
    public class FilterData
    {
        public HashSet<string> UnitsOfMeasurement { get; set; }

        public FilterData(HashSet<string> unitsOfMeasurement)
        {
            UnitsOfMeasurement = unitsOfMeasurement;
        }

        public FilterData()
        {
            UnitsOfMeasurement = new HashSet<string>();
        }
    }

}
