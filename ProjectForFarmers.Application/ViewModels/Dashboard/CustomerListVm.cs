using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.ViewModels.Dashboard
{
    public class CustomerListVm
    {
        public List<string> Customers {  get; set; }

        public CustomerListVm()
        {
            Customers = new List<string>();
        }
    }

}
