using FarmersMarketplace.Domain.Feedbacks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarmersMarketplace.Persistence.EntityTypeConfigurations
{
    public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder.ToTable("Feedbacks");

            builder.HasKey(feedback => feedback.Id);
            builder.HasOne(product => product.Customer)
                .WithMany()
                .HasForeignKey(feedback => feedback.CustomerId);

            builder.HasIndex(feedback => feedback.Id).IsUnique();
        }
    }
}
