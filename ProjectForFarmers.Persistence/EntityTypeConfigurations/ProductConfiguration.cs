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
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(product => product.Id);
            builder.Property(subcategory => subcategory.Name).IsRequired();
            builder.Property(subcategory => subcategory.Description).IsRequired();

            builder.Property(product => product.Description).IsRequired();
            builder.Property(product => product.Description).IsRequired();
            builder.Property(product => product.Description).IsRequired();
            builder.Property(product => product.Description).IsRequired();
            builder.Property(product => product.Description).IsRequired();
            builder.Property(product => product.Description).IsRequired();
            builder.Property(product => product.Description).IsRequired();
            builder.Property(product => product.Description).IsRequired();
            builder.Property(product => product.Description).IsRequired();
            builder.Property(product => product.Description).IsRequired();
            builder.Property(product => product.Description).IsRequired();
            builder.Property(product => product.Description).IsRequired();

            builder.HasOne(f => f.Category)
                .WithOne()
                .HasForeignKey<Product>(product => product.CategoryId)
                .IsRequired();


            builder.HasIndex(product => product.Id).IsUnique();
        }
    }

}
