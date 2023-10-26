using Agroforum.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agroforum.Persistence.EntityTypeConfigurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(account => account.Id);
            builder.Property(account => account.Name).HasMaxLength(60).IsRequired();
            builder.Property(account => account.Surname).HasMaxLength(60).IsRequired();

            builder.HasIndex(account => account.Id).IsUnique();
        }
    }
}
