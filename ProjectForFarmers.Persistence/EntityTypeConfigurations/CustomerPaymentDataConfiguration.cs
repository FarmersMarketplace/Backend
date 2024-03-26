using FarmersMarketplace.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Persistence.EntityTypeConfigurations
{
    public class CustomerPaymentDataConfiguration : IEntityTypeConfiguration<CustomerPaymentData>
    {
        public void Configure(EntityTypeBuilder<CustomerPaymentData> builder)
        {
            builder.ToTable("CustomerPaymentData");

            builder.HasIndex(data => data.Id).IsUnique();
        }
    }

}
