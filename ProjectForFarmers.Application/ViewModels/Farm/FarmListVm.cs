using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.ViewModels.Farm
{
    public class FarmListVm
    {
        public List<FarmLookupVm> Farms { get; set; }

        public FarmListVm()
        {
            Farms = new List<FarmLookupVm>();
        }
    }
}
