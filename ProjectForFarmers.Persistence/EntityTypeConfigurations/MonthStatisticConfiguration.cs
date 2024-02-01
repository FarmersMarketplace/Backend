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
    public class MonthStatisticConfiguration : IEntityTypeConfiguration<MonthStatistic>
    {
        public void Configure(EntityTypeBuilder<MonthStatistic> builder)
        {
            builder.ToTable("MonthesStatistics");

            builder.HasKey(statistic => statistic.Id);
            builder.Property(statistic => statistic.StartDate).IsRequired();
            builder.Property(statistic => statistic.EndDate).IsRequired();
            builder.Property(statistic => statistic.TotalRevenue).IsRequired();
            builder.Property(statistic => statistic.TotalRevenueChangePercentage).IsRequired();
            builder.Property(statistic => statistic.Producer).IsRequired();
            builder.Property(statistic => statistic.ProducerId).IsRequired();

            builder.HasOne(f => f.BookedOrdersStatistic)
                .WithOne()
                .HasForeignKey<MonthStatistic>(f => f.BookedOrdersStatisticId);

            builder.HasOne(f => f.CompletedOrdersStatistic)
                .WithOne()
                .HasForeignKey<MonthStatistic>(f => f.CompletedOrdersStatisticId);

            builder.HasOne(f => f.ProcessingOrdersStatistic)
                .WithOne()
                .HasForeignKey<MonthStatistic>(f => f.ProcessingOrdersStatisticId);

            builder.HasOne(f => f.NewOrdersStatistic)
                .WithOne()
                .HasForeignKey<MonthStatistic>(f => f.NewOrdersStatisticId);

            builder.HasOne(f => f.TotalActivityStatistic)
                .WithOne()
                .HasForeignKey<MonthStatistic>(f => f.TotalActivityStatisticId);

            builder.HasIndex(account => account.Id).IsUnique();
        }
    }

}
