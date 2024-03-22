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
    public class PaymentDataConfiguration : IEntityTypeConfiguration<ProducerPaymentData>
    {
        public void Configure(EntityTypeBuilder<ProducerPaymentData> builder)
        {
            builder.ToTable("PaymentData");

            builder.HasKey(data => data.Id);
            builder.Property(data => data.CardNumber).IsRequired();
            builder.Property(data => data.AccountNumber).IsRequired();
            builder.Property(data => data.BankUSREOU).IsRequired();
            builder.Property(data => data.BIC).IsRequired();
            builder.Property(data => data.HolderFullName).IsRequired();

            builder.HasIndex(data => data.Id).IsUnique();
        }
    }

}
