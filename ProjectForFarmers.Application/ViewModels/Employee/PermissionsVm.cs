using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.ViewModels.Employee
{
    public class PermissionsVm
    {
        public Guid Id { get; set; }
        public bool UpdateFarmInformation { get; set; }
        public bool CreateProduct { get; set; }
        public bool UpdateProduct { get; set; }
        public bool DeleteProduct { get; set; }
        public bool ListProductForSale { get; set; }
        public bool WithdrawProductFromSale { get; set; }
        public bool AddEmployee { get; set; }
        public bool UpdateEmployeePermissions { get; set; }
        public bool DeleteEmployee { get; set; }
    }
}
