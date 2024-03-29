using FarmersMarketplace.Domain;
using FarmersMarketplace.Domain.Account;
using FarmersMarketplace.Domain.Orders;
using FarmersMarketplace.Domain.Payment;
using Microsoft.EntityFrameworkCore;
using DayOfWeek = FarmersMarketplace.Domain.DayOfWeek;

namespace FarmersMarketplace.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Customer> Customers { get; set; }
        DbSet<Seller> Sellers { get; set; }
        DbSet<Farmer> Farmers { get; set; }
        DbSet<Address> ProducerAddresses { get; set; }
        DbSet<CustomerAddress> CustomerAddresses { get; set; }
        DbSet<Farm> Farms { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<DayOfWeek> DaysOfWeek { get; set; }
        DbSet<Schedule> Schedules { get; set; }
        DbSet<MonthStatistic> MonthesStatistics { get; set; }
        DbSet<OrderItem> OrdersItems { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Subcategory> Subcategories { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<ProducerPaymentData> ProducerPaymentData { get; set; }
        DbSet<CustomerPaymentData> CustomerPaymentData { get; set; }
        DbSet<FarmLog> FarmsLogs { get; set; }
        Task<int> SaveChangesAsync(CancellationToken token);
        Task<int> SaveChangesAsync();
    }
}
