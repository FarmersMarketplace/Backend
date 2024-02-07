﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProjectForFarmers.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Persistence.EntityTypeConfigurations
{
    public class FarmLogConfiguration : IEntityTypeConfiguration<FarmLog>
    {
        public void Configure(EntityTypeBuilder<FarmLog> builder)
        {
            builder.ToTable("Accounts");

            builder.HasKey(log => log.Id);
            builder.Property(log => log.Message).IsRequired();
            builder.Property(log => log.CreationDate).IsRequired();
            builder.HasOne<Farm>()
                .WithMany()
                .HasForeignKey(log => log.FarmId);

            builder.HasIndex(log => log.Id).IsUnique();
        }
    }

}
