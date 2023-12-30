using System.Text;
using Microsoft.IdentityModel.Tokens;
using Agroforum.Persistence;
using Agroforum.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Agroforum.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Agroforum.WebApi.Middlewares;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace Agroforum.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();
            ConfigureApp(app);


            app.MapGet("/", () => "Hello World!");

            app.Run();

        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
            //var dbName = Environment.GetEnvironmentVariable("DB_NAME");
            //var userName = Environment.GetEnvironmentVariable("DB_USER");
            //var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
            //string connectionString = $"Host={dbHost};Database={dbName};Username={userName};Password={dbPassword};";
            string connectionString = configuration.GetConnectionString("RenderConnection");
            services.AddPersistence(connectionString);
            
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            Log.Information("The program has started.");

            services.AddApplication();
            services.AddControllers(options =>
            {
                options.Filters.Add(new ProducesAttribute("application/json"));
            });

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

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<MainDbContext>();
                if (context.Database.GetPendingMigrations().Any())
                    context.Database.Migrate();
            }

            AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
            {
                Log.Information("Program has exited successfully.");
                Log.CloseAndFlush();
            };


            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseCors("AllowAll"); //change in future
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