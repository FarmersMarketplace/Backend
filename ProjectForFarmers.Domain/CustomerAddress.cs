using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Domain
{
    public class CustomerAddress : Address
    {
        public string? Apartment { get; set; }
    }

}
