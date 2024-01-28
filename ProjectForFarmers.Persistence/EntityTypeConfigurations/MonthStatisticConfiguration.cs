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

            builder.HasOne<OrderGroupStatistic>()
                .WithOne()
                .HasForeignKey<MonthStatistic>(f => f.BookedOrdersStatisticId);

            builder.HasOne<OrderGroupStatistic>()
                .WithOne()
                .HasForeignKey<MonthStatistic>(f => f.CompleteOrdersStatisticId);

            builder.HasOne<OrderGroupStatistic>()
                .WithOne()
                .HasForeignKey<MonthStatistic>(f => f.ProcessingOrdersStatisticId);

            builder.HasOne<OrderGroupStatistic>()
                .WithOne()
                .HasForeignKey<MonthStatistic>(f => f.NewOrdersStatisticId);

            builder.HasOne<OrderGroupStatistic>()
                .WithOne()
                .HasForeignKey<MonthStatistic>(f => f.TotalActivityStatisticId);

            builder.HasIndex(account => account.Id).IsUnique();
        }
    }

}
