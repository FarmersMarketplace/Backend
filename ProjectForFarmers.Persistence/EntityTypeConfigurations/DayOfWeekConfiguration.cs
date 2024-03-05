using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FarmersMarketplace.Domain;
using DayOfWeek = FarmersMarketplace.Domain.DayOfWeek;

namespace FarmersMarketplace.Persistence.EntityTypeConfigurations
{
    public class DayOfWeekConfiguration : IEntityTypeConfiguration<DayOfWeek>
    {
        public void Configure(EntityTypeBuilder<DayOfWeek> builder)
        {
            builder.ToTable("DaysOfWeek");

            builder.HasKey(dayOfWeek => dayOfWeek.Id);
            builder.Property(dayOfWeek => dayOfWeek.IsOpened).IsRequired();

            builder.HasIndex(dayOfWeek => dayOfWeek.Id).IsUnique();
        }
    }

}
