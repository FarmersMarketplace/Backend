using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectForFarmers.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Persistence.EntityTypeConfigurations
{
    public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            builder.ToTable("Schedules");

            builder.HasKey(dayOfWeek => dayOfWeek.Id);

            builder.HasOne(f => f.Monday)
                .WithOne()
                .HasForeignKey<Schedule>(f => f.MondayId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.Tuesday)
                .WithOne()
                .HasForeignKey<Schedule>(f => f.TuesdayId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.Wednesday)
                .WithOne()
                .HasForeignKey<Schedule>(f => f.WednesdayId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.Thursday)
                .WithOne()
                .HasForeignKey<Schedule>(f => f.ThursdayId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.Friday)
                .WithOne()
                .HasForeignKey<Schedule>(f => f.FridayId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.Saturday)
                .WithOne()
                .HasForeignKey<Schedule>(f => f.SaturdayId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.Sunday)
                .WithOne()
                .HasForeignKey<Schedule>(f => f.SundayId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(schedule => schedule.Id).IsUnique();
        }
    }

}
