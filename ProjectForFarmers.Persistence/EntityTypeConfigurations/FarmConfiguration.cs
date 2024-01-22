using ProjectForFarmers.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Persistence.EntityTypeConfigurations
{
    public class FarmConfiguration : IEntityTypeConfiguration<Farm>
    {
        public void Configure(EntityTypeBuilder<Farm> builder)
        {
            builder.ToTable("Farms");

            builder.HasKey(farm => farm.Id);
            builder.Property(farm => farm.Name).IsRequired();
            builder.Property(farm => farm.AddressId).IsRequired();

            builder.HasOne<Address>()
                .WithOne()
                .HasForeignKey<Farm>(f => f.AddressId);
        }
    }
}
