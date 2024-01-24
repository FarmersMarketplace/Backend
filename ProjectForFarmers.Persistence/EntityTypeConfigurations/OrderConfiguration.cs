using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProjectForFarmers.Domain;

namespace ProjectForFarmers.Persistence.EntityTypeConfigurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(order => order.Id);

            builder.Property(order => order.Number)
                .IsRequired()
                .HasMaxLength(7)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn()
                .HasComputedColumnSql("RIGHT('0000000' + CAST(CONVERT(NVARCHAR(7), [Number]) AS NVARCHAR(7)), 7)");

            builder.Property(account => account.CustomerName).IsRequired();
            builder.Property(account => account.CustomerPhone).IsRequired();
            builder.Property(account => account.CustomerEmail).IsRequired();
            builder.Property(account => account.PaymentTotal).IsRequired();
            builder.Property(account => account.PaymentType).IsRequired();

            builder.HasOne(order => order.Customer)
                .WithMany()
                .HasForeignKey(order => order.CustomerId);

            builder.HasOne(order => order.Farm)
                .WithMany()
                .HasForeignKey(order => order.FarmId);
            
            builder.HasIndex(order => order.Id).IsUnique();
        }
    }

}
