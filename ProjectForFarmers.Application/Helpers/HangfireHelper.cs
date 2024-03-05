using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Application.Services.Business;
using Serilog;

namespace FarmersMarketplace.Application.Helpers
{
    public static class HangfireHelper
    {
        public static DateTime LastDayOfCurrentMonth => new DateTime(DateTimeOffset.UtcNow.Year,
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
            Log.Information("1");
            BackgroundJob.Schedule(() => UpdateStatistics(), LastDayOfNextMonth());

            await StatisticService.UpdateAllStatistics(LastDayOfCurrentMonth);
        }

        public static DateTimeOffset LastDayOfNextMonth()
        {
            return LastDayOfCurrentMonth.AddMonths(1);
        }

        public static void RegisterTasks(IServiceProvider services)
        {
            var scope = services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            DbContext = serviceProvider.GetRequiredService<IApplicationDbContext>();

            StatisticService = new StatisticService(DbContext);

            BackgroundJob.Schedule(() => UpdateStatistics(), LastDayOfCurrentMonth);
        }
    }

}
