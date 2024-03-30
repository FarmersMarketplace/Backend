using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Application.Services.Business;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace FarmersMarketplace.Application.Helpers
{
    public static class HangfireHelper
    {
        public static DateTime LastDayOfCurrentMonth => new DateTime(DateTime.UtcNow.Year,
                DateTime.UtcNow.Month,
                1, 0, 0, 0)
                    .AddMonths(1)
                    .AddSeconds(-1);
        public static IApplicationDbContext DbContext { get; set; }
        private static StatisticService StatisticService { get; set; }

        static HangfireHelper()
        {
        }

        public static async Task UpdateStatistics()
        {
            BackgroundJob.Schedule(() => UpdateStatistics(), LastDayOfNextMonth());

            await StatisticService.UpdateAllStatistics(LastDayOfCurrentMonth);
        }

        public static DateTime LastDayOfNextMonth()
        {
            int year = LastDayOfCurrentMonth.Year;
            int month = LastDayOfCurrentMonth.Month;

            int nextMonth = month == 12 ? 1 : month + 1;
            int nextYear = month == 12 ? year + 1 : year;

            DateTime lastDayOfNextMonth = new DateTime(nextYear, nextMonth, 1).AddMonths(1).AddDays(-1);

            return lastDayOfNextMonth;
        }

        public static void RegisterTasks(IServiceProvider services)
        {
            using (var connection = JobStorage.Current.GetConnection())
            {
                var monitoringApi = JobStorage.Current.GetMonitoringApi();
                var scheduledCount = monitoringApi.ScheduledCount();

                if (scheduledCount > 0)
                {
                    return;
                }
            }

            var scope = services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            DbContext = serviceProvider.GetRequiredService<IApplicationDbContext>();

            StatisticService = new StatisticService(DbContext);

            BackgroundJob.Schedule(() => UpdateStatistics(), LastDayOfCurrentMonth);
        }
    }

}
