using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Domain
{
    public class Schedule
    {
        public Guid? Id { get; set; }
        public Guid? MondayId { get; set; }
        public Guid? TuesdayId { get; set; }
        public Guid? WednesdayId { get; set; }
        public Guid? ThursdayId { get; set; }
        public Guid? FridayId { get; set; }
        public Guid? SaturdayId { get; set; }
        public Guid? SundayId { get; set; }
        public DayOfWeek Monday { get; set; }
        public DayOfWeek Tuesday { get; set; }
        public DayOfWeek Wednesday { get; set; }
        public DayOfWeek Thursday { get; set; }
        public DayOfWeek Friday { get; set; }
        public DayOfWeek Saturday { get; set; }
        public DayOfWeek Sunday { get; set; }

        public Schedule()
        {
            Monday = new DayOfWeek(Guid.NewGuid());
            Tuesday = new DayOfWeek(Guid.NewGuid());
            Wednesday = new DayOfWeek(Guid.NewGuid());
            Thursday = new DayOfWeek(Guid.NewGuid());
            Friday = new DayOfWeek(Guid.NewGuid());
            Saturday = new DayOfWeek(Guid.NewGuid());
            Sunday = new DayOfWeek(Guid.NewGuid());

            MondayId = Monday.Id;
            TuesdayId = Tuesday.Id;
            WednesdayId = Wednesday.Id;
            ThursdayId = Thursday.Id;
            FridayId = Friday.Id;
            SundayId = Sunday.Id;
            SaturdayId = Saturday.Id;
        }

    }

}
