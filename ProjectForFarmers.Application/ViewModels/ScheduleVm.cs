using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarmersMarketplace.Application.ViewModels.Farm;

namespace FarmersMarketplace.Application.ViewModels
{
    public class ScheduleVm
    {
        public DayOfWeekVm Monday { get; set; }
        public DayOfWeekVm Tuesday { get; set; }
        public DayOfWeekVm Wednesday { get; set; }
        public DayOfWeekVm Thursday { get; set; }
        public DayOfWeekVm Friday { get; set; }
        public DayOfWeekVm Saturday { get; set; }
        public DayOfWeekVm Sunday { get; set; }
    }

}
