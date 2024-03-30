namespace FarmersMarketplace.Application.DataTransferObjects
{
    public class DayOfWeekDto
    {
        public bool IsOpened { get; set; }
        public byte? StartHour { get; set; }
        public byte? StartMinute { get; set; }
        public byte? EndHour { get; set; }
        public byte? EndMinute { get; set; }
    }

}
