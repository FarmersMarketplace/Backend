using Agroforum.Application.Interfaces;
using Agroforum.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agroforum.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<PostgresDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });
            services.AddScoped<IAgroforumDbContext>(provider => provider.GetService<PostgresDbContext>());


            return services;
        }
    }
}
