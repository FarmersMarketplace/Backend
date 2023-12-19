using Agroforum.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace Agroforum.Persistence.EntityTypeConfigurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Addresses");

            builder.HasKey(address => address.Id);
            builder.Property(address => address.Region).IsRequired();
            builder.Property(address => address.Settlement).IsRequired();
            builder.Property(address => address.Street).IsRequired();
            builder.Property(address => address.HouseNumber).IsRequired();
            builder.Property(address => address.PostalCode).IsRequired();

            builder.HasIndex(address => address.Id).IsUnique();
        }
    }
}
