using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.ViewModels.Dashboard
{
    public class AutocompleteOptionsListVm
    {
        public List<string> Customers {  get; set; }

        public AutocompleteOptionsListVm()
        {
            Customers = new List<string>();
        }
    }

}
