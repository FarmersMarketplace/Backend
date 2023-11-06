using System.Text;
using Microsoft.IdentityModel.Tokens;
using Agroforum.Persistence;
using Agroforum.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Agroforum.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

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
            var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
            var dbName = Environment.GetEnvironmentVariable("DB_NAME");
            var userName = Environment.GetEnvironmentVariable("DB_USER");
            var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
            //string connectionString = $"Host={dbHost};Database={dbName};Username={userName};Password={dbPassword};";
            var connectionString = configuration.GetConnectionString("PostgresConnection");

            services.AddPersistence(connectionString);
            services.AddApplication();
            services.AddControllers();

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

                var context = services.GetRequiredService<PostgresDbContext>();
                if (context.Database.GetPendingMigrations().Any())
                    context.Database.Migrate();
            }

            //app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseCors("AllowAll"); //change in future
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}