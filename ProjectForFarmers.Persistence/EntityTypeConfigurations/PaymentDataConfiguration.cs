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
    public class PaymentDataConfiguration : IEntityTypeConfiguration<PaymentData>
    {
        public void Configure(EntityTypeBuilder<PaymentData> builder)
        {
            builder.ToTable("PaymantData");

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
