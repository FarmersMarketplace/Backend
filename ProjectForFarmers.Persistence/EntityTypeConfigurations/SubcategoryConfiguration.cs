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
    public class SubcategoryConfiguration : IEntityTypeConfiguration<Subcategory>
    {
        public void Configure(EntityTypeBuilder<Subcategory> builder)
        {
            builder.ToTable("Subcategories");

            builder.HasKey(subcategory => subcategory.Id);

            builder.Property(subcategory => subcategory.Name).IsRequired();
            builder.HasOne(subcategory => subcategory.Category)
                .WithMany(category => category.Subcategories)
                .HasForeignKey(subcategory => subcategory.CategoryId);
        }
    }

}
