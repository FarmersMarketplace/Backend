using ProjectForFarmers.Application.Interfaces;
using ProjectForFarmers.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MainDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<MainDbContext>());


            return services;
        }
    }
}
