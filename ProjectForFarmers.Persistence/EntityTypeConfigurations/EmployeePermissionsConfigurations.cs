using ProjectForFarmers.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ProjectForFarmers.Persistence.EntityTypeConfigurations
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
