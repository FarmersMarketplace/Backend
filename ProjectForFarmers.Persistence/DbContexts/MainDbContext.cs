using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Domain;
using FarmersMarketplace.Domain.Accounts;
using FarmersMarketplace.Domain.Orders;
using FarmersMarketplace.Domain.Payment;
using FarmersMarketplace.Persistence.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;
using DayOfWeek = FarmersMarketplace.Domain.DayOfWeek;

namespace FarmersMarketplace.Persistence.DbContexts
{
    public class MainDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<Farmer> Farmers { get; set; }
        public DbSet<Address> ProducerAddresses { get; set; }
        public DbSet<CustomerAddress> CustomerAddresses { get; set; }
        public DbSet<Farm> Farms { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<DayOfWeek> DaysOfWeek { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<MonthStatistic> MonthesStatistics { get; set; }
        public DbSet<OrderItem> OrdersItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProducerPaymentData> ProducerPaymentData { get; set; }
        public DbSet<CustomerPaymentData> CustomerPaymentData { get; set; }
        public DbSet<FarmLog> FarmsLogs { get; set; }

        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new FarmerConfiguration());
            modelBuilder.ApplyConfiguration(new SellerConfiguration());
            modelBuilder.ApplyConfiguration(new AddressConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerAddressConfiguration());
            modelBuilder.ApplyConfiguration(new DayOfWeekConfiguration());
            modelBuilder.ApplyConfiguration(new ScheduleConfiguration());
            modelBuilder.ApplyConfiguration(new FarmConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
            modelBuilder.ApplyConfiguration(new MonthStatisticConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new SubcategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProducerPaymentDataConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerPaymentDataConfiguration());
            modelBuilder.ApplyConfiguration(new FarmLogConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}