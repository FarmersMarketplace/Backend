using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Domain
{
    public class DayOfWeek
    {
        public Guid Id { get; set; }
        private string startHour;
        private string startMinute;
        private string endHour;
        private string endMinute;

        public string StartHour
        {
            get { return startHour; }
            set
            {
                if (IsValidHour(value))
                    startHour = value;
                else
                    throw new ArgumentException("Invalid value for StartHour.");
            }
        }

        public string StartMinute
        {
            get { return startMinute; }
            set
            {
                if (IsValidMinute(value))
                    startMinute = value;
                else
                    throw new ArgumentException("Invalid value for StartMinute.");
            }
        }

        public string EndHour
        {
            get { return endHour; }
            set
            {
                if (IsValidHour(value))
                    endHour = value;
                else
                    throw new ArgumentException("Invalid value for EndHour.");
            }
        }

        public string EndMinute
        {
            get { return endMinute; }
            set
            {
                if (IsValidMinute(value))
                    endMinute = value;
                else
                    throw new ArgumentException("Invalid value for EndMinute.");
            }
        }

        public DayOfWeek()
        {
            StartHour = "00";
            StartMinute = "00";
            EndHour = "00";
            EndMinute = "00";
        }

        public DayOfWeek(string startHour, string startMinute, string endHour, string endMinute)
        {
            var startTimeInMinutes = int.Parse(startHour) * 60 + int.Parse(startMinute);
            var endTimeInMinutes = int.Parse(endHour) * 60 + int.Parse(endMinute);

            if (endTimeInMinutes <= startTimeInMinutes)
                throw new ArgumentException("End time must be greater than start time.");

            StartHour = startHour;
            StartMinute = startMinute;
            EndHour = endHour;
            EndMinute = endMinute;
        }

        private bool IsValidHour(string hour)
        {
            int hourValue;
            return int.TryParse(hour, out hourValue) && hourValue >= 0 && hourValue < 24;
        }

        private bool IsValidMinute(string minute)
        {
            int minuteValue;
            return int.TryParse(minute, out minuteValue) && minuteValue >= 0 && minuteValue < 60;
        }
    }


}
