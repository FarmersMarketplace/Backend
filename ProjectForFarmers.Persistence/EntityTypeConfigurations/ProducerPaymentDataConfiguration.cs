using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarmersMarketplace.Domain.Payment;

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
