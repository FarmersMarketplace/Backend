using System.Text;
using Microsoft.IdentityModel.Tokens;
using ProjectForFarmers.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ProjectForFarmers.WebApi.Middlewares;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using ProjectForFarmers.Persistence;
using Hangfire;
using Hangfire.PostgreSql;
using ProjectForFarmers.Application.Helpers;
using ProjectForFarmers.Application.Interfaces;

namespace ProjectForFarmers.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();
            ConfigureApp(app);

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("LocalConnection");

            services.AddPersistence(connectionString);
            Log.Logger = new LoggerConfiguration().WriteTo.PostgreSQL(connectionString, "Logs", needAutoCreateTable: true)
               .MinimumLevel.Information().CreateLogger();

            Log.Information("The program has started.");

            services.AddApplication(configuration);
            services.AddControllers(options =>
            {
                options.Filters.Add(new ProducesAttribute("application/json"));
            });

            services.AddHangfire(config =>
                config.UsePostgreSqlStorage(c =>
                    c.UseNpgsqlConnection(connectionString)));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
                {
                    var key = Encoding.UTF8.GetBytes(configuration["Auth:Secret"]);
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Auth:Issuer"],
                        ValidAudience = configuration["Auth:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key),

                        ValidateLifetime = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                });

            services.AddSwaggerGen(config =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                config.IncludeXmlComments(xmlPath);
                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Swagger",
                    Version = "v1",
                });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

        }

        private static void ConfigureApp(WebApplication app)
        {
            if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHangfireServer();
            app.UseHangfireDashboard();

            HangfireHelper.RegisterTasks(app.Services);

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<IApplicationDbContext>();

                context.Database.ExecuteSqlRaw("DELETE FROM logs WHERE timestamp < NOW() - INTERVAL '2 months'");
            }
        

            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}