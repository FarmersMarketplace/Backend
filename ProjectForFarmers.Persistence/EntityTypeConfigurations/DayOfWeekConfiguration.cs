using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectForFarmers.Domain;
using DayOfWeek = ProjectForFarmers.Domain.DayOfWeek;

namespace ProjectForFarmers.Persistence.EntityTypeConfigurations
{
    public class DayOfWeekConfiguration : IEntityTypeConfiguration<DayOfWeek>
    {
        public void Configure(EntityTypeBuilder<DayOfWeek> builder)
        {
            builder.ToTable("DaysOfWeek");

            builder.HasKey(dayOfWeek => dayOfWeek.Id);
            builder.Property(dayOfWeek => dayOfWeek.StartMinute).HasMaxLength(2).IsRequired();
            builder.Property(dayOfWeek => dayOfWeek.StartHour).HasMaxLength(2).IsRequired();
            builder.Property(dayOfWeek => dayOfWeek.EndMinute).HasMaxLength(2).IsRequired();
            builder.Property(dayOfWeek => dayOfWeek.EndHour).HasMaxLength(2).IsRequired();

            builder.HasIndex(dayOfWeek => dayOfWeek.Id).IsUnique();
        }
    }

}
