using Agroforum.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agroforum.Application.Models;

namespace Agroforum.Persistence.EntityTypeConfigurations
{
    public class UnconfirmedPhoneConfiguration : IEntityTypeConfiguration<UnconfirmedPhone>
    {
        public void Configure(EntityTypeBuilder<UnconfirmedPhone> builder)
        {
            builder.HasKey(up => up.AccountId);
            builder.Property(up => up.Number).IsRequired();
            builder.Property(up => up.Code).IsRequired();
            builder.Property(up => up.Deadline).IsRequired();

            builder.HasIndex(up => up.AccountId).IsUnique();
        }
    }
}
