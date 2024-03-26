using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FarmersMarketplace.Domain;

namespace FarmersMarketplace.Persistence.EntityTypeConfigurations
{
    public class FarmLogConfiguration : IEntityTypeConfiguration<FarmLog>
    {
        public void Configure(EntityTypeBuilder<FarmLog> builder)
        {
            builder.ToTable("FarmsLogs");

            builder.HasKey(log => log.Id);
            builder.Property(log => log.Message).IsRequired();
            builder.Property(log => log.CreationDate).IsRequired();

            builder.HasIndex(log => log.Id).IsUnique();
        }
    }

}
