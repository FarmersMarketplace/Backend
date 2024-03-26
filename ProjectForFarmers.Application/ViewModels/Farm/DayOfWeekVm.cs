using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.ViewModels.Farm
{
    public class DayOfWeekVm
    {
        public bool IsOpened { get; set; }
        public byte? StartHour { get; set; }
        public byte? StartMinute { get; set; }
        public byte? EndHour { get; set; }
        public byte? EndMinute { get; set; }
    }

}
