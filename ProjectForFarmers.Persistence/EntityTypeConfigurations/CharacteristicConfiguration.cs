using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProjectForFarmers.Domain;

namespace ProjectForFarmers.Persistence.EntityTypeConfigurations
{
    public class CharacteristicConfiguration : IEntityTypeConfiguration<Characteristic>
    {
        public void Configure(EntityTypeBuilder<Characteristic> builder)
        {
            builder.ToTable("Subcategories");

            builder.HasKey(characteristic => characteristic.Id);
            builder.Property(characteristic => characteristic.Name).IsRequired();
            builder.Property(characteristic => characteristic.Value).IsRequired();

            builder.HasOne<Product>()
               .WithOne()
               .HasForeignKey<Characteristic>(c => c.ProductId)
               .IsRequired();

            builder.HasIndex(characteristic => characteristic.Id).IsUnique();
        }
    }

}
