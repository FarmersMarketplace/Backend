using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.ViewModels.Farm
{
    public class FarmLookupVm
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? AvatarName { get; set; }

        public FarmLookupVm()
        {
            
        }
    }
}
