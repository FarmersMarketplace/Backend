using FarmersMarketplace.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Persistence.EntityTypeConfigurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.UseTpcMappingStrategy();

            builder.HasKey(account => account.Id);
            builder.Property(account => account.Id).ValueGeneratedNever();

            builder.HasIndex(account => account.Id).IsUnique();
        }
    }

}
