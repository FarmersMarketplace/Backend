using FarmersMarketplace.Domain.Feedbacks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarmersMarketplace.Persistence.EntityTypeConfigurations
{
    public class ProductFeedbackCollectionConfiguration : IEntityTypeConfiguration<ProductFeedbackCollection>
    {
        public void Configure(EntityTypeBuilder<ProductFeedbackCollection> builder)
        {
            builder.ToTable("ProductFeedbackCollections");

            builder.HasKey(collection => collection.Id);
            builder.HasIndex(collection => collection.Id).IsUnique();
        }
    }
}
