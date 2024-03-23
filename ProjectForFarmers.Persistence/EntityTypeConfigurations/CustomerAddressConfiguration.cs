using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarmersMarketplace.Domain;

namespace FarmersMarketplace.Persistence.EntityTypeConfigurations
{
    public class CustomerAddressConfiguration : IEntityTypeConfiguration<CustomerAddress>
    {
        public void Configure(EntityTypeBuilder<CustomerAddress> builder)
        {
            builder.ToTable("CustomerAddresses");

            builder.HasKey(address => address.Id);
            builder.Property(address => address.Region).HasMaxLength(30).IsRequired();
            builder.Property(address => address.District).HasMaxLength(30).IsRequired();
            builder.Property(address => address.Settlement).HasMaxLength(50).IsRequired();
            builder.Property(address => address.Street).HasMaxLength(50).IsRequired();
            builder.Property(address => address.HouseNumber).HasMaxLength(10).IsRequired();
            builder.Property(address => address.PostalCode).HasMaxLength(5).IsRequired();

            builder.HasIndex(address => address.Id).IsUnique();
        }
    }

}
