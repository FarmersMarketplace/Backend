using Agroforum.Application.Interfaces;
using Agroforum.Application.Models;
using Agroforum.Domain;
using Agroforum.Persistence.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Agroforum.Persistence.DbContexts
{
    public class PostgresDbContext : DbContext, IAgroforumDbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<UnconfirmedPhone> UnconfirmedPhones { get; set; }

        public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new UnconfirmedPhoneConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
