using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Domain
{
    public class Schedule
    {
        public Guid Id { get; set; }
        public DayOfWeek Monday { get; set; }
        public DayOfWeek Tuesday { get; set; }
        public DayOfWeek Wednesday { get; set; }
        public DayOfWeek Thursday { get; set; }
        public DayOfWeek Friday { get; set; }
        public DayOfWeek Saturday { get; set; }
        public DayOfWeek Sunday { get; private set; }

        public Guid MondayId { get; set; }
        public Guid TuesdayId { get; set; }
        public Guid WednesdayId { get; set; }
        public Guid ThursdayId { get; set; }
        public Guid FridayId { get; set; }
        public Guid SaturdayId { get; set; }
        public Guid SundayId { get; set; }
    }

}
