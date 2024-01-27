using Microsoft.EntityFrameworkCore;
using ProjectForFarmers.Application.Interfaces;
using ProjectForFarmers.Application.ViewModels.Farm;
using ProjectForFarmers.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.Services.Business
{
    public class StatisticService
    {
        public readonly IApplicationDbContext DbContext;

        public StatisticService(IApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task UpdateAllStatistics(DateTimeOffset lastDateOfMonth)
        {
            var farms = DbContext.Farms.ToArray();

            foreach (var farm in farms)
            {
                var farmMonthStatistic = await CalculateFarmStatisticForMonth(farm.Id, lastDateOfMonth);
                DbContext.MonthesStatistics.Add(farmMonthStatistic);
            }

            var sellers = DbContext.Accounts.Where(a => a.Role == Role.Seller).ToArray();

            foreach (var seller in sellers)
            {
                var sellerMonthStatistic = await CalculateSellerStatisticForMonth(seller.Id, lastDateOfMonth);
                DbContext.MonthesStatistics.Add(sellerMonthStatistic);
            }

            DbContext.SaveChangesAsync();
        }

        private async Task<MonthStatistic> CalculateSellerStatisticForMonth(Guid sellerId, DateTimeOffset lastDateOfMonth)
        {
            var orders = DbContext.Orders.Where(o => o.Producer == Producer.Seller
                && o.ProducerId == sellerId
                && o.CreationDate.Month == lastDateOfMonth.Month).ToArray();

            var bookedOrders = orders.Where(o => o.Status != OrderStatus.Completed).ToArray();
            var completedOrders = orders.Where(o => o.Status == OrderStatus.Completed).ToArray();
            var processingOrders = orders.Where(o => o.Status == OrderStatus.Processing).ToArray();
            var newOrders = orders.Where(o => o.Status == OrderStatus.New).ToArray();
            var totalActivityOrders = orders;

            DateTimeOffset firstDayOfCurrentMonth = new DateTimeOffset(lastDateOfMonth.Year, lastDateOfMonth.Month, 1, 0, 0, 0, lastDateOfMonth.Offset);
            DateTimeOffset lastDayOfPreviousMonth = firstDayOfCurrentMonth.AddDays(-1);


            var previousStatistic = await DbContext.MonthesStatistics.FirstOrDefaultAsync(m => m.Producer == Producer.Seller
                && m.ProducerId == sellerId
                && m.StartDate.Year == lastDayOfPreviousMonth.Year
                && m.StartDate.Month == lastDayOfPreviousMonth.Month
                && m.StartDate.Day == 1
                && m.EndDate.Year == lastDayOfPreviousMonth.Year
                && m.EndDate.Month == lastDayOfPreviousMonth.Month
                && m.EndDate.Day == lastDayOfPreviousMonth.Day);

            var existsOrdersForPreviousMonth = DbContext.Orders.Any(o => o.CreationDate.Year >= previousStatistic.StartDate.Year
                    && o.CreationDate.Month >= previousStatistic.StartDate.Month
                    && o.CreationDate.Day >= previousStatistic.StartDate.Day
                    && o.CreationDate.Year <= previousStatistic.EndDate.Year
                    && o.CreationDate.Month <= previousStatistic.EndDate.Month
                    && o.CreationDate.Day <= previousStatistic.EndDate.Day);

            if (previousStatistic == null && existsOrdersForPreviousMonth)
            {
                previousStatistic = await CalculateFarmStatisticForMonth(sellerId, lastDayOfPreviousMonth);

                DbContext.MonthesStatistics.Add(previousStatistic);
                await DbContext.SaveChangesAsync();

            }

            var bookedOrdersStat = new OrderGroupStatistic
            {
                Id = Guid.NewGuid(),
                Count = (uint)bookedOrders.Length,
                PercentageChange = await CalculatePercentageChanges(bookedOrders.Length, previousStatistic.BookedOrders.Count)
            };

            var completedOrdersStat = new OrderGroupStatistic
            {
                Id = Guid.NewGuid(),
                Count = (uint)bookedOrders.Length,
                PercentageChange = await CalculatePercentageChanges(completedOrders.Length, previousStatistic.CompletedOrders.Count)
            };

            var processingOrdersStat = new OrderGroupStatistic
            {
                Id = Guid.NewGuid(),
                Count = (uint)processingOrders.Length,
                PercentageChange = await CalculatePercentageChanges(processingOrders.Length, previousStatistic.ProcessingOrders.Count)
            };

            var newOrdersStat = new OrderGroupStatistic
            {
                Id = Guid.NewGuid(),
                Count = (uint)newOrders.Length,
                PercentageChange = await CalculatePercentageChanges(newOrders.Length, previousStatistic.NewOrders.Count)
            };

            var totalActivityOrdersOrdersStat = new OrderGroupStatistic
            {
                Id = Guid.NewGuid(),
                Count = (uint)totalActivityOrders.Length,
                PercentageChange = await CalculatePercentageChanges(totalActivityOrders.Length, previousStatistic.TotalActivity.Count)
            };

            var result = new MonthStatistic
            {
                Id = Guid.NewGuid(),
                ProducerId = sellerId,
                Producer = Producer.Seller,
                StartDate = new DateTime(lastDateOfMonth.Year, lastDateOfMonth.Month, lastDateOfMonth.Day, 0, 0, 0),
                EndDate = new DateTime(lastDateOfMonth.Year, lastDateOfMonth.Month, lastDateOfMonth.Day, 23, 59, 59),
                BookedOrders = bookedOrdersStat,
                CompletedOrders = completedOrdersStat,
                ProcessingOrders = processingOrdersStat,
                NewOrders = newOrdersStat,
                TotalActivity = totalActivityOrdersOrdersStat
            };

            return result;
        }

        private async Task<MonthStatistic> CalculateFarmStatisticForMonth(Guid farmId, DateTimeOffset lastDateOfMonth)
        {
            var orders = DbContext.Orders.Where(o => o.Producer == Producer.Farm 
                && o.ProducerId == farmId
                && o.CreationDate.Month == lastDateOfMonth.Month).ToArray();

            var bookedOrders = orders.Where(o => o.Status != OrderStatus.Completed).ToArray();
            var completedOrders = orders.Where(o => o.Status == OrderStatus.Completed).ToArray();
            var processingOrders = orders.Where(o => o.Status == OrderStatus.Processing).ToArray();
            var newOrders = orders.Where(o => o.Status == OrderStatus.New).ToArray();
            var totalActivityOrders = orders;

            DateTimeOffset firstDayOfCurrentMonth = new DateTimeOffset(lastDateOfMonth.Year, lastDateOfMonth.Month, 1, 0, 0, 0, lastDateOfMonth.Offset);
            DateTimeOffset lastDayOfPreviousMonth = firstDayOfCurrentMonth.AddDays(-1);


            var previousStatistic = await DbContext.MonthesStatistics.FirstOrDefaultAsync(m => m.Producer == Producer.Farm
                && m.ProducerId == farmId
                && m.StartDate.Year == lastDayOfPreviousMonth.Year
                && m.StartDate.Month == lastDayOfPreviousMonth.Month
                && m.StartDate.Day == 1
                && m.EndDate.Year == lastDayOfPreviousMonth.Year
                && m.EndDate.Month == lastDayOfPreviousMonth.Month
                && m.EndDate.Day == lastDayOfPreviousMonth.Day);

            var existsOrdersForPreviousMonth = DbContext.Orders.Any(o => o.CreationDate.Year >= previousStatistic.StartDate.Year
                    && o.CreationDate.Month >= previousStatistic.StartDate.Month
                    && o.CreationDate.Day >= previousStatistic.StartDate.Day
                    && o.CreationDate.Year <= previousStatistic.EndDate.Year
                    && o.CreationDate.Month <= previousStatistic.EndDate.Month
                    && o.CreationDate.Day <= previousStatistic.EndDate.Day);

            if (previousStatistic == null && existsOrdersForPreviousMonth)
            {
                previousStatistic = await CalculateFarmStatisticForMonth(farmId, lastDayOfPreviousMonth);

                DbContext.MonthesStatistics.Add(previousStatistic);
                await DbContext.SaveChangesAsync();

            }

            var bookedOrdersStat = new OrderGroupStatistic
            {
                Id = Guid.NewGuid(),
                Count = (uint)bookedOrders.Length,
                PercentageChange = await CalculatePercentageChanges(bookedOrders.Length, previousStatistic.BookedOrders.Count)
            };

            var completedOrdersStat = new OrderGroupStatistic
            {
                Id = Guid.NewGuid(),
                Count = (uint)bookedOrders.Length,
                PercentageChange = await CalculatePercentageChanges(completedOrders.Length, previousStatistic.CompletedOrders.Count)
            };

            var processingOrdersStat = new OrderGroupStatistic
            {
                Id = Guid.NewGuid(),
                Count = (uint)processingOrders.Length,
                PercentageChange = await CalculatePercentageChanges(processingOrders.Length, previousStatistic.ProcessingOrders.Count)
            };

            var newOrdersStat = new OrderGroupStatistic
            {
                Id = Guid.NewGuid(),
                Count = (uint)newOrders.Length,
                PercentageChange = await CalculatePercentageChanges(newOrders.Length, previousStatistic.NewOrders.Count)
            };

            var totalActivityOrdersOrdersStat = new OrderGroupStatistic
            {
                Id = Guid.NewGuid(),
                Count = (uint)totalActivityOrders.Length,
                PercentageChange = await CalculatePercentageChanges(totalActivityOrders.Length, previousStatistic.TotalActivity.Count)
            };

            var result = new MonthStatistic
            {
                Id = Guid.NewGuid(),
                ProducerId = farmId,
                Producer = Producer.Farm,
                StartDate = new DateTime(lastDateOfMonth.Year, lastDateOfMonth.Month, lastDateOfMonth.Day, 0, 0, 0),
                EndDate = new DateTime(lastDateOfMonth.Year, lastDateOfMonth.Month, lastDateOfMonth.Day, 23, 59, 59),
                BookedOrders = bookedOrdersStat,
                CompletedOrders = completedOrdersStat,
                ProcessingOrders = processingOrdersStat,
                NewOrders = newOrdersStat,
                TotalActivity = totalActivityOrdersOrdersStat
            };

            return result;
        }

        private async Task<float> CalculatePercentageChanges(int currentCount, uint previousCount)
        {
            float curPercent = currentCount * 100 / previousCount;

            return curPercent - 100;
        }
    }

}
