using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Cache.Providers;
using FarmersMarketplace.Domain;
using FarmersMarketplace.Domain.Accounts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Order = FarmersMarketplace.Domain.Orders.Order;

namespace FarmersMarketplace.Cache
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var multiplexerConfiguration = ConfigurationOptions.Parse(configuration.GetConnectionString("Redis"));
                return ConnectionMultiplexer.Connect(multiplexerConfiguration);
            });

            services.AddTransient<ICacheProvider<Product>, ProductCacheProvider>();
            services.AddTransient<ICacheProvider<Order>, OrderCacheProvider>();
            services.AddTransient<ICacheProvider<Seller>, SellerCacheProvider>();
            services.AddTransient<ICacheProvider<Farm>, FarmCacheProvider>();
            services.AddTransient<ICacheProvider<Customer>, CustomerCacheProvider>();
            services.AddTransient<ICacheProvider<Farmer>, FarmerCacheProvider>();

            return services;
        }
    }

}
