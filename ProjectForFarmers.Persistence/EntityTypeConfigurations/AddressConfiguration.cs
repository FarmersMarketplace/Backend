﻿using ProjectForFarmers.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace ProjectForFarmers.Persistence.EntityTypeConfigurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Addresses");

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