using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agroforum.Application.ViewModels.Farm
{
    public class FarmLookupVm
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public FarmLookupVm(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
