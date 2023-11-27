using Agroforum.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agroforum.Persistence.EntityTypeConfigurations
{
    public class EmployeePermissionsConfigurations : IEntityTypeConfiguration<EmployeePermissions>
    {
        public void Configure(EntityTypeBuilder<EmployeePermissions> builder)
        {
            builder.ToTable("EmployeesPermissions");

            builder.HasKey(permission => permission.Id);
            builder.HasOne<Account>()
               .WithMany()
               .HasForeignKey(permission => permission.AccountId)
               .OnDelete(DeleteBehavior.Cascade); 

            builder.HasOne<Farm>()
                   .WithMany()
                   .HasForeignKey(permission => permission.FarmId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(permission => permission.Id).IsUnique();
        }
    }
}
