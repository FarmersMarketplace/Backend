using FarmersMarketplace.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Persistence.EntityTypeConfigurations
{
    public class FarmConfiguration : IEntityTypeConfiguration<Farm>
    {
        public void Configure(EntityTypeBuilder<Farm> builder)
        {
            builder.ToTable("Farms");

            builder.HasKey(farm => farm.Id);
            builder.Property(farm => farm.Name).HasMaxLength(50).IsRequired();
            builder.Property(farm => farm.AddressId).IsRequired();
            builder.Property(farm => farm.OwnerId).IsRequired();
            builder.Property(farm => farm.ScheduleId).IsRequired();
            builder.Property(farm => farm.CreationDate).IsRequired();

            builder.HasOne(f => f.Address)
                 .WithOne()
                 .HasForeignKey<Farm>(f => f.AddressId)
                 .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.Owner)
                 .WithMany()
                 .HasForeignKey(f => f.OwnerId)
                 .IsRequired();

            builder.HasOne(f => f.Schedule)
                 .WithOne()
                 .HasForeignKey<Farm>(f => f.ScheduleId)
                 .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.PaymentData)
                 .WithOne()
                 .HasForeignKey<Farm>(f => f.PaymentDataId)
                 .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(farm => farm.Id).IsUnique();
        }
    }
}
