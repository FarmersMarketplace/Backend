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

            builder.HasOne(s => s.BookedOrdersStatistic)
                .WithOne()
                .HasForeignKey<MonthStatistic>(s => s.BookedOrdersStatisticId);

            builder.HasOne(s => s.CompletedOrdersStatistic)
                .WithOne()
                .HasForeignKey<MonthStatistic>(s => s.CompletedOrdersStatisticId);

            builder.HasOne(s => s.ProcessingOrdersStatistic)
                .WithOne()
                .HasForeignKey<MonthStatistic>(s => s.ProcessingOrdersStatisticId);

            builder.HasOne(s => s.NewOrdersStatistic)
                .WithOne()
                .HasForeignKey<MonthStatistic>(s => s.NewOrdersStatisticId);

            builder.HasOne(s => s.TotalActivityStatistic)
                .WithOne()
                .HasForeignKey<MonthStatistic>(s => s.TotalActivityStatisticId);

            builder.HasIndex(account => account.Id).IsUnique();
        }
    }

}
