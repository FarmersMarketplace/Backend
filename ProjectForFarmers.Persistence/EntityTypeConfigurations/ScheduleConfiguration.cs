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
                .IsRequired();

            builder.HasOne(f => f.Tuesday)
                .WithOne()
                .HasForeignKey<Schedule>(f => f.TuesdayId)
                .IsRequired();

            builder.HasOne(f => f.Wednesday)
                .WithOne()
                .HasForeignKey<Schedule>(f => f.WednesdayId)
                .IsRequired();

            builder.HasOne(f => f.Thursday)
                .WithOne()
                .HasForeignKey<Schedule>(f => f.ThursdayId)
                .IsRequired();

            builder.HasOne(f => f.Friday)
                .WithOne()
                .HasForeignKey<Schedule>(f => f.FridayId)
                .IsRequired();

            builder.HasOne(f => f.Saturday)
                .WithOne()
                .HasForeignKey<Schedule>(f => f.SaturdayId)
                .IsRequired();

            builder.HasOne(f => f.Sunday)
                .WithOne()
                .HasForeignKey<Schedule>(f => f.SundayId)
                .IsRequired();

            builder.HasIndex(schedule => schedule.Id).IsUnique();
        }
    }

}
