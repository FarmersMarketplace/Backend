using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarmersMarketplace.Domain.Accounts;

namespace FarmersMarketplace.Persistence.EntityTypeConfigurations
{
    public class SellerConfiguration : IEntityTypeConfiguration<Seller>
    {
        public void Configure(EntityTypeBuilder<Seller> builder)
        {
            builder.UseTpcMappingStrategy().ToTable("Sellers");

            builder.Property(seller => seller.Name).HasMaxLength(50).IsRequired();
            builder.Property(seller => seller.Surname).HasMaxLength(50).IsRequired();

            builder.HasOne(seller => seller.Address)
                 .WithOne()
                 .HasForeignKey<Seller>(seller => seller.AddressId);

            builder.HasOne(seller => seller.PaymentData)
                 .WithOne()
                 .HasForeignKey<Seller>(seller => seller.PaymentDataId);

            builder.HasOne(seller => seller.Schedule)
                .WithOne()
                .HasForeignKey<Seller>(seller => seller.ScheduleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(seller => seller.Id).IsUnique();
        }
    }

}
