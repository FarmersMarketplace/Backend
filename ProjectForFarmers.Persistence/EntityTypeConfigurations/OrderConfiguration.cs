using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FarmersMarketplace.Domain;

namespace FarmersMarketplace.Persistence.EntityTypeConfigurations
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
                .UseSerialColumn();

            builder.Property(order => order.CreationDate).IsRequired();
            builder.Property(order => order.ReceiveDate).IsRequired();
            builder.Property(order => order.TotalPayment).IsRequired();
            builder.Property(order => order.PaymentType).IsRequired();
            builder.Property(order => order.PaymentStatus).IsRequired();
            builder.Property(order => order.ReceivingMethod).IsRequired();

            builder.HasOne(order => order.Customer)
                .WithMany() 
                .HasForeignKey(order => order.CustomerId)
                .IsRequired();

            builder.HasOne(order => order.DeliveryPoint)
                .WithMany()
                .HasForeignKey(order => order.DeliveryPointId);

            builder.HasMany(o => o.Items)
               .WithOne()
               .HasForeignKey(oi => oi.OrderId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(order => order.Id).IsUnique();
            builder.HasIndex(order => order.CreationDate).IsDescending();
        }
    }

}
