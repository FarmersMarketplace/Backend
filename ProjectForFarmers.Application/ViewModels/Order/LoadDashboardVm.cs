using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.ViewModels.Order
{
    public class LoadDashboardVm
    {
        public List<MonthStatisticLookupVm> DateRanges {  get; set; }
        public DashboardVm CurrentMonthDashboard { get; set; }
    }

}
