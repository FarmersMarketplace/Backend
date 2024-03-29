using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarmersMarketplace.Domain.Orders;

namespace FarmersMarketplace.Persistence.EntityTypeConfigurations
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


            builder.HasIndex(item => item.OrderId).IsUnique();
        }
    }

}
