using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarmersMarketplace.Application.ViewModels.Order;

namespace FarmersMarketplace.Application.ViewModels.Dashboard
{
    public class LoadDashboardVm
    {
        public List<MonthStatisticLookupVm> DateRanges { get; set; }
        public DashboardVm CurrentMonthDashboard { get; set; }
    }

}
