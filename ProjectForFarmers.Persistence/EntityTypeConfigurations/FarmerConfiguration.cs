using FarmersMarketplace.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FarmersMarketplace.Persistence.EntityTypeConfigurations
{
    public class FarmerConfiguration : IEntityTypeConfiguration<Farmer>
    {
        public void Configure(EntityTypeBuilder<Farmer> builder)
        {
            builder.ToTable("Farmers");

            builder.HasKey(farmer => farmer.Id);
            builder.Property(farmer => farmer.Name).HasMaxLength(50).IsRequired();
            builder.Property(farmer => farmer.Surname).HasMaxLength(50).IsRequired();

            builder.HasOne(farmer => farmer.Address)
                 .WithOne()
                 .HasForeignKey<Farmer>(farmer => farmer.AddressId);

            builder.HasOne(farmer => farmer.PaymentData)
                 .WithOne()
                 .HasForeignKey<Farmer>(farmer => farmer.PaymentDataId);

            builder.HasIndex(farmer => farmer.Id).IsUnique();
        }
    }

}
