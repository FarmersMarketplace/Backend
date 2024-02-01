using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.ViewModels
{
    public class AddressVm
    {
        public string Region { get; set; }
        public string District { get; set; }
        public string Settlement { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string PostalCode { get; set; }
        public string Note { get; set; }
        public double Longtitude { get; set; }
        public double Latitude { get; set; }
    }

}
