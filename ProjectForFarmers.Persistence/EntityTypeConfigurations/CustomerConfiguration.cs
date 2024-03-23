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
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");

            builder.HasKey(customer => customer.Id);
            builder.Property(customer => customer.Name).HasMaxLength(50).IsRequired();
            builder.Property(customer => customer.Surname).HasMaxLength(50).IsRequired();

            builder.HasOne(customer => customer.Address)
                 .WithOne()
                 .HasForeignKey<Customer>(customer => customer.AddressId);

            builder.HasOne(customer => customer.PaymentData)
                 .WithOne()
                 .HasForeignKey<Customer>(customer => customer.PaymentDataId);

            builder.HasIndex(customer => customer.Id).IsUnique();
        }
    }

}
