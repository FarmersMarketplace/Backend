using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProjectForFarmers.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Persistence.EntityTypeConfigurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrdersItems");

            builder.HasKey(item => item.Id);
            builder.Property(item => item.ProductId).IsRequired();
            builder.Property(item => item.OrderId).IsRequired();
            builder.Property(item => item.Count).IsRequired();
            builder.Property(item => item.TotalPrice).IsRequired();

            builder.HasOne<Order>()
                .WithMany()
                .HasForeignKey(item => item.OrderId);

            throw new Exception("Add product connection.");

            builder.HasIndex(account => account.Id).IsUnique();
        }
    }

}
