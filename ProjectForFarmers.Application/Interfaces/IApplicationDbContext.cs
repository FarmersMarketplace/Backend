using ProjectForFarmers.Domain;
using Microsoft.EntityFrameworkCore;
using DayOfWeek = ProjectForFarmers.Domain.DayOfWeek;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ProjectForFarmers.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Account> Accounts { get; set; }
        DbSet<Address> Addresses { get; set; }
        DbSet<Farm> Farms { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<DayOfWeek> DaysOfWeek { get; set; }
        DbSet<Schedule> Schedules { get; set; }
        DbSet<MonthStatistic> MonthesStatistics { get; set; }
        DbSet<OrderItem> OrdersItems { get; set; }
        Task<int> SaveChangesAsync(CancellationToken token);
        Task<int> SaveChangesAsync();
        DatabaseFacade Database { get; }
    }
}
