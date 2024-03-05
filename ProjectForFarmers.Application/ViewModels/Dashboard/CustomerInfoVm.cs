using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.ViewModels.Dashboard
{
    public class CustomerInfoVm
    {
        public decimal Payment { get; set; }
        public float PaymentPercentage { get; set; }
        public string Name { get; set; }
    }

}
