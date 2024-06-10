namespace FarmersMarketplace.Application.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTime? ToUniversalTime(this DateTime? date)
        {
            return date != null ? ((DateTime)date).ToUniversalTime() : null;
        }
    }

}
