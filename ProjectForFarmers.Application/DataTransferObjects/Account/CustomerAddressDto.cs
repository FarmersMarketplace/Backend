using FarmersMarketplace.Application.DataTransferObjects.Farm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.DataTransferObjects.Account
{
    public class CustomerAddressDto : AddressDto
    {
        public string? Apartment { get; set; }
    }

}
