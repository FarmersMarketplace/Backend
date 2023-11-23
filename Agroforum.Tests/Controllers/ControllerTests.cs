using Agroforum.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Agroforum.Tests.Controllers
{
    public class ControllerTests
    {
        protected IConfiguration Configuration { get; set; }
        protected PostgresDbContext DbContext { get; set; }

        public ControllerTests()
        {
            string path = Directory.GetCurrentDirectory() + "\\..\\..\\..\\app.filenesting.json";
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(path)
                .Build();

            var dbContextOptions = new DbContextOptionsBuilder<PostgresDbContext>()
           .UseNpgsql(Configuration.GetConnectionString("TestDbConnection"))
           .Options;

            DbContext = new PostgresDbContext(dbContextOptions);

            DbContext.Database.EnsureCreated();
        }
    }
}
