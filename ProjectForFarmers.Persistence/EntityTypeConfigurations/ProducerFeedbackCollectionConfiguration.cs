using FarmersMarketplace.Domain.Feedbacks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarmersMarketplace.Persistence.EntityTypeConfigurations
{
    internal class ProducerFeedbackCollectionConfiguration : IEntityTypeConfiguration<ProducerFeedbackCollection>
    {
        public void Configure(EntityTypeBuilder<ProducerFeedbackCollection> builder)
        {
            builder.ToTable("ProducerFeedbackCollections");

            builder.HasKey(collection => collection.Id);
            builder.HasIndex(collection => collection.Id).IsUnique();
        }
    }
}
