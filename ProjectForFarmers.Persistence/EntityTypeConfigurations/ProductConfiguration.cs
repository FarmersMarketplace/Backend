using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FarmersMarketplace.Domain;

namespace FarmersMarketplace.Persistence.EntityTypeConfigurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(product => product.Id);
            builder.Property(product => product.Name).IsRequired();
            builder.Property(product => product.Description).IsRequired();
            builder.Property(product => product.ArticleNumber).IsRequired();
            builder.Property(product => product.Producer).IsRequired();
            builder.Property(product => product.ProducerId).IsRequired();
            builder.Property(product => product.PackagingType).IsRequired();
            builder.Property(product => product.UnitOfMeasurement).IsRequired();
            builder.Property(product => product.PricePerOne).IsRequired();
            builder.Property(product => product.MinPurchaseQuantity).IsRequired();
            builder.Property(product => product.Count).IsRequired();

            builder.HasOne(product => product.Category)
                .WithMany()
                .HasForeignKey(product => product.CategoryId);

            builder.HasOne(product => product.Subcategory)
                .WithMany()
                .HasForeignKey(product => product.SubcategoryId);

            builder.HasIndex(product => product.Id).IsUnique();
            builder.HasIndex(order => order.CreationDate).IsDescending();
        }
    }

}
