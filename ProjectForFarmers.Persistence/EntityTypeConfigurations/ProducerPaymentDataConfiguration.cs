using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FarmersMarketplace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Persistence.EntityTypeConfigurations
{
    public class ProducerPaymentDataConfiguration : IEntityTypeConfiguration<ProducerPaymentData>
    {
        public void Configure(EntityTypeBuilder<ProducerPaymentData> builder)
        {
            builder.ToTable("ProducerPaymentData");

            builder.HasIndex(data => data.Id).IsUnique();
        }
    }

}
