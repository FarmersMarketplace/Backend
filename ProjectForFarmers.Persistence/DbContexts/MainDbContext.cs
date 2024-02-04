using ProjectForFarmers.Application.Interfaces;
using ProjectForFarmers.Domain;
using ProjectForFarmers.Persistence.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;
using DayOfWeek = ProjectForFarmers.Domain.DayOfWeek;

namespace ProjectForFarmers.Persistence.DbContexts
{
    public class MainDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Farm> Farms { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<DayOfWeek> DaysOfWeek { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<MonthStatistic> MonthesStatistics { get; set; }
        public DbSet<OrderItem> OrdersItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PaymentData> PaymentData { get; set; }

        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new AddressConfiguration());
            modelBuilder.ApplyConfiguration(new DayOfWeekConfiguration());
            modelBuilder.ApplyConfiguration(new ScheduleConfiguration());
            modelBuilder.ApplyConfiguration(new FarmConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
            modelBuilder.ApplyConfiguration(new MonthStatisticConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new SubcategoryConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentDataConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}