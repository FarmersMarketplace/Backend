using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectForFarmers.Application.ViewModels.Order;

namespace ProjectForFarmers.Application.ViewModels.Dashboard
{
    public class LoadDashboardVm
    {
        public List<MonthStatisticLookupVm> DateRanges { get; set; }
        public DashboardVm CurrentMonthDashboard { get; set; }
    }

}
