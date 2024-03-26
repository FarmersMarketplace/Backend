using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FarmersMarketplace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Persistence.EntityTypeConfigurations
{
    public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            builder.ToTable("Schedules");

            builder.HasKey(schedule => schedule.Id);

            builder.HasOne(s => s.Monday)
                .WithOne()
                .HasForeignKey<Schedule>(s => s.MondayId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.Tuesday)
                .WithOne()
                .HasForeignKey<Schedule>(s => s.TuesdayId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.Wednesday)
                .WithOne()
                .HasForeignKey<Schedule>(s => s.WednesdayId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.Thursday)
                .WithOne()
                .HasForeignKey<Schedule>(s => s.ThursdayId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.Friday)
                .WithOne()
                .HasForeignKey<Schedule>(s => s.FridayId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.Saturday)
                .WithOne()
                .HasForeignKey<Schedule>(s => s.SaturdayId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.Sunday)
                .WithOne()
                .HasForeignKey<Schedule>(s => s.SundayId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(schedule => schedule.Id).IsUnique();
        }
    }

}
