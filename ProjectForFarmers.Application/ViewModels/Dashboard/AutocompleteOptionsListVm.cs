using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.ViewModels.Dashboard
{
    public class OptionListVm
    {
        public HashSet<string> Options {  get; set; }

        public OptionListVm()
        {
            Options = new HashSet<string>();
        }
    }

}
