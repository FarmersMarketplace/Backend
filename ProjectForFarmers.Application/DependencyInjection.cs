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

namespace ProjectForFarmers.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IFarmService, FarmService>();
            return services;
        }
    }
}
