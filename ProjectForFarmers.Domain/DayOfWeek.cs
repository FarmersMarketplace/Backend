using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Domain
{
    public class DayOfWeek
    {
        public Guid Id { get; set; }
        public bool IsOpened { get; set; }
        private byte? startHour;
        private byte? startMinute;
        private byte? endHour;
        private byte? endMinute;

        public byte? StartHour
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

        public byte? StartMinute
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

        public byte? EndHour
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

        public byte? EndMinute
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
            IsOpened = false;
            StartHour = null;
            StartMinute = null;
            EndHour = null;
            EndMinute = null;
        }

        public DayOfWeek(Guid id)
        {
            Id = id;
            IsOpened = false;
            StartHour = null;
            StartMinute = null;
            EndHour = null;
            EndMinute = null;
        }

        public DayOfWeek(bool isOpened, byte? startHour, byte? startMinute, byte? endHour, byte? endMinute)
        {
            IsOpened = isOpened;

            if (startHour != null && startMinute != null && endHour != null && endMinute != null) 
            {
                int startTimeInMinutes = (int)(startHour * 60 + startMinute);
                int endTimeInMinutes = (int)(endHour * 60 + endMinute);

                if (endTimeInMinutes <= startTimeInMinutes)
                    throw new ArgumentException("End time must be greater than start time.");
            }

            StartHour = startHour;
            StartMinute = startMinute;
            EndHour = endHour;
            EndMinute = endMinute;
        }

        private bool IsValidHour(byte? hour)
        {
            return hour == null || (hour >= 0 && hour < 24);
        }

        private bool IsValidMinute(byte? minute)
        {
            return (minute == null) || (minute >= 0 && minute < 60);
        }
    }


}
