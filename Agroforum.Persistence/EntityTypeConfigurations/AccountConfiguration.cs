using Agroforum.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Agroforum.Persistence.EntityTypeConfigurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Accounts");

            builder.HasKey(account => account.Id);
            builder.Property(account => account.Name).HasMaxLength(30).IsRequired();
            builder.Property(account => account.Surname).HasMaxLength(30).IsRequired();
            builder.Property(account => account.Password).HasMaxLength(64);

            builder.HasIndex(account => account.Id).IsUnique();
        }
    }
}
