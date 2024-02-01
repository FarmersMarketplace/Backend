using ProjectForFarmers.Application.Services;
using ProjectForFarmers.Application.Services.Auth;
using ProjectForFarmers.Application.Services.Business;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectForFarmers.Application.Mappings;
using AutoMapper;

namespace ProjectForFarmers.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IFarmService, FarmService>();
            services.AddTransient<IOrderService, OrderService>();

            services.AddAutoMapper(config =>
            {
                config.AddProfile(new AuthMappingProfile());
                config.AddProfile(new FarmMappingProfile());
                config.AddProfile(new OrderMappingProfile());
            });

            return services;
        }
    }
}
