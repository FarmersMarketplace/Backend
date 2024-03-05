using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.DataTransferObjects.Farm
{
    public class ScheduleDto
    {
        public DayOfWeekDto Monday { get; set; }
        public DayOfWeekDto Tuesday { get; set; }
        public DayOfWeekDto Wednesday { get; set; }
        public DayOfWeekDto Thursday { get; set; }
        public DayOfWeekDto Friday { get; set; }
        public DayOfWeekDto Saturday { get; set; }
        public DayOfWeekDto Sunday { get; set; }
    }

}
