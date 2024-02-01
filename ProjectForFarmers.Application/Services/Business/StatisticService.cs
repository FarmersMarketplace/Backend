using Microsoft.EntityFrameworkCore;
using ProjectForFarmers.Application.Interfaces;
using ProjectForFarmers.Domain;

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
                var farmMonthStatistic = await CalculateStatisticForMonth(farm.Id, Producer.Farm, lastDateOfMonth);
                DbContext.MonthesStatistics.Add(farmMonthStatistic);
            }

            var sellers = DbContext.Accounts.Where(a => a.Role == Role.Seller).ToArray();

            foreach (var seller in sellers)
            {
                var sellerMonthStatistic = await CalculateStatisticForMonth(seller.Id, Producer.Seller, lastDateOfMonth);
                DbContext.MonthesStatistics.Add(sellerMonthStatistic);
            }

            await DbContext.SaveChangesAsync();
        }

        public async Task<MonthStatistic> CalculateStatisticForMonth(Guid producerId, Producer producer, DateTimeOffset lastDateOfMonth)
        {
            DateTimeOffset firstDayOfCurrentMonth = new DateTimeOffset(lastDateOfMonth.Year, lastDateOfMonth.Month, 1, 0, 0, 0, lastDateOfMonth.Offset);
            DateTimeOffset lastDayOfPreviousMonth = firstDayOfCurrentMonth.AddDays(-1);
            DateTimeOffset firstDayOfPreviousMonth = firstDayOfCurrentMonth.AddMonths(-1);

            var previousStatistic = await GetPreviousStatistic(producerId, producer, lastDayOfPreviousMonth);

            if (previousStatistic == null)
            {
                var existsOrdersForPreviousMonth = await ExistsOrdersForPeriodOfTime(producerId, firstDayOfPreviousMonth, lastDayOfPreviousMonth);

                if (existsOrdersForPreviousMonth)
                {
                    previousStatistic = await CalculateStatisticForMonth(producerId, producer, lastDayOfPreviousMonth);

                    DbContext.MonthesStatistics.Add(previousStatistic);
                    await DbContext.SaveChangesAsync();
                }
                else
                {
                    return await CalculateMonthStatisticForFirstMonth(producerId, producer, lastDateOfMonth);
                }
            }

            return await CalculateMonthStatisticForNotFirstMonth(producerId, producer, lastDateOfMonth, previousStatistic, lastDayOfPreviousMonth);
        }

        private async Task<MonthStatistic> CalculateMonthStatisticForNotFirstMonth(Guid producerId, Producer producer, DateTimeOffset lastDateOfMonth, MonthStatistic previousStatistic, DateTimeOffset lastDayOfPreviousMonth)
        {
            var orders = DbContext.Orders.Where(o => o.Producer == producer
                && o.ProducerId == producerId
                && o.CreationDate.Month == lastDateOfMonth.Month).ToArray();

            var bookedOrders = orders.Where(o => o.Status != OrderStatus.Completed).ToArray();
            var completedOrders = orders.Where(o => o.Status == OrderStatus.Completed).ToArray();
            var processingOrders = orders.Where(o => o.Status == OrderStatus.Processing).ToArray();
            var newOrders = orders.Where(o => o.Status == OrderStatus.New).ToArray();
            var totalActivityOrders = orders;

            var bookedOrdersStat = new OrderGroupStatistic
            {
                Id = Guid.NewGuid(),
                Count = (uint)bookedOrders.Length,
                PercentageChange = await CalculatePercentageChanges(bookedOrders.Length, previousStatistic.BookedOrdersStatistic.Count)
            };

            var completedOrdersStat = new OrderGroupStatistic
            {
                Id = Guid.NewGuid(),
                Count = (uint)bookedOrders.Length,
                PercentageChange = await CalculatePercentageChanges(completedOrders.Length, previousStatistic.CompletedOrdersStatistic.Count)
            };

            var processingOrdersStat = new OrderGroupStatistic
            {
                Id = Guid.NewGuid(),
                Count = (uint)processingOrders.Length,
                PercentageChange = await CalculatePercentageChanges(processingOrders.Length, previousStatistic.ProcessingOrdersStatistic.Count)
            };

            var newOrdersStat = new OrderGroupStatistic
            {
                Id = Guid.NewGuid(),
                Count = (uint)newOrders.Length,
                PercentageChange = await CalculatePercentageChanges(newOrders.Length, previousStatistic.NewOrdersStatistic.Count)
            };

            var totalActivityOrdersOrdersStat = new OrderGroupStatistic
            {
                Id = Guid.NewGuid(),
                Count = (uint)totalActivityOrders.Length,
                PercentageChange = await CalculatePercentageChanges(totalActivityOrders.Length, previousStatistic.TotalActivityStatistic.Count)
            };

            var totalRevenu = orders.Sum(order => order.TotalPayment);

            var customerWithHighestPayment = (
                from order in orders
                group order by order.CustomerId into customerGroup
                let totalPayment = customerGroup.Sum(order => order.TotalPayment)
                orderby totalPayment descending
                select new
                {
                    CustomerId = customerGroup.Key,
                    TotalPayment = totalPayment
                }).FirstOrDefault();

            Guid? customerId = customerWithHighestPayment?.CustomerId;
            decimal sum = customerWithHighestPayment?.TotalPayment ?? 0;

            var result = new MonthStatistic
            {
                Id = Guid.NewGuid(),
                ProducerId = producerId,
                Producer = producer,
                StartDate = new DateTime(lastDateOfMonth.Year, lastDateOfMonth.Month, 1, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(lastDateOfMonth.Year, lastDateOfMonth.Month, lastDateOfMonth.Day, 23, 59, 59, DateTimeKind.Utc),
                BookedOrdersStatistic = bookedOrdersStat,
                CompletedOrdersStatistic = completedOrdersStat,
                ProcessingOrdersStatistic = processingOrdersStat,
                NewOrdersStatistic = newOrdersStat,
                TotalActivityStatistic = totalActivityOrdersOrdersStat,
                BookedOrdersStatisticId = bookedOrdersStat.Id,
                CompletedOrdersStatisticId = completedOrdersStat.Id,
                ProcessingOrdersStatisticId = processingOrdersStat.Id,
                NewOrdersStatisticId = newOrdersStat.Id,
                TotalActivityStatisticId = totalActivityOrdersOrdersStat.Id,
                TotalRevenue = totalRevenu,
                TotalRevenueChangePercentage = await CalculatePercentageChanges(totalRevenu, previousStatistic.TotalRevenue),
                CustomerWithHighestPaymentId = customerId,
                HighestCustomerPayment = sum,
                HighestCustomerPaymentPercentage = await CalculatePaymentPercentageChanges(sum, totalRevenu)
            };

            return result;
        }

        private async Task<float> CalculatePaymentPercentageChanges(decimal sum, decimal totalRevenu)
        {
            return (float)((sum*100)/totalRevenu);
        }

        private async Task<MonthStatistic> CalculateMonthStatisticForFirstMonth(Guid producerId, Producer producer, DateTimeOffset lastDateOfMonth)
        {
            var orders = DbContext.Orders.Where(o => o.Producer == producer
                && o.ProducerId == producerId
                && o.CreationDate.Month == lastDateOfMonth.Month
                && o.CreationDate.Year == lastDateOfMonth.Year).ToArray();

            var bookedOrders = orders.Where(o => o.Status != OrderStatus.Completed).ToArray();
            var completedOrders = orders.Where(o => o.Status == OrderStatus.Completed).ToArray();
            var processingOrders = orders.Where(o => o.Status == OrderStatus.Processing).ToArray();
            var newOrders = orders.Where(o => o.Status == OrderStatus.New).ToArray();
            var totalActivityOrders = orders;

            var bookedOrdersStat = new OrderGroupStatistic
            {
                Id = Guid.NewGuid(),
                Count = (uint)bookedOrders.Length,
                PercentageChange = bookedOrders.Length > 0 ? 100 : 0
            };

            var completedOrdersStat = new OrderGroupStatistic
            {
                Id = Guid.NewGuid(),
                Count = (uint)bookedOrders.Length,
                PercentageChange = completedOrders.Length > 0 ? 100 : 0
            };

            var processingOrdersStat = new OrderGroupStatistic
            {
                Id = Guid.NewGuid(),
                Count = (uint)processingOrders.Length,
                PercentageChange = processingOrders.Length > 0 ? 100 : 0
            };

            var newOrdersStat = new OrderGroupStatistic
            {
                Id = Guid.NewGuid(),
                Count = (uint)newOrders.Length,
                PercentageChange = newOrders.Length > 0 ? 100 : 0
            };

            var totalActivityOrdersOrdersStat = new OrderGroupStatistic
            {
                Id = Guid.NewGuid(),
                Count = (uint)totalActivityOrders.Length,
                PercentageChange = totalActivityOrders.Length > 0 ? 100 : 0
            };

            var totalRevenu = orders.Sum(order => order.TotalPayment);

            var customerWithHighestPayment = (
                from order in orders
                group order by order.CustomerId into customerGroup
                let totalPayment = customerGroup.Sum(order => order.TotalPayment)
                orderby totalPayment descending
                select new
                {
                    CustomerId = customerGroup.Key,
                    TotalPayment = totalPayment
                }).FirstOrDefault();

            Guid? customerId = customerWithHighestPayment?.CustomerId;
            decimal sum = customerWithHighestPayment?.TotalPayment ?? 0;

            var result = new MonthStatistic
            {
                Id = Guid.NewGuid(),
                ProducerId = producerId,
                Producer = producer,
                StartDate = new DateTime(lastDateOfMonth.Year, lastDateOfMonth.Month, 1, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(lastDateOfMonth.Year, lastDateOfMonth.Month, lastDateOfMonth.Day, 23, 59, 59, DateTimeKind.Utc),
                BookedOrdersStatistic = bookedOrdersStat,
                CompletedOrdersStatistic = completedOrdersStat,
                ProcessingOrdersStatistic = processingOrdersStat,
                NewOrdersStatistic = newOrdersStat,
                TotalActivityStatistic = totalActivityOrdersOrdersStat,
                BookedOrdersStatisticId = bookedOrdersStat.Id,
                CompletedOrdersStatisticId = completedOrdersStat.Id,
                ProcessingOrdersStatisticId = processingOrdersStat.Id,
                NewOrdersStatisticId = newOrdersStat.Id,
                TotalActivityStatisticId = totalActivityOrdersOrdersStat.Id,
                TotalRevenue = totalRevenu,
                TotalRevenueChangePercentage = 100,
                CustomerWithHighestPaymentId = customerId,
                HighestCustomerPayment = sum,
                HighestCustomerPaymentPercentage = await CalculatePaymentPercentageChanges(sum, totalRevenu)
            };

            return result;
        }

        private async Task<MonthStatistic> GetPreviousStatistic(Guid producerId, Producer producer, DateTimeOffset lastDayOfPreviousMonth)
        {
            var previousStatistic = await DbContext.MonthesStatistics
                .Include(m => m.BookedOrdersStatistic)
                .Include(m => m.CompletedOrdersStatistic)
                .Include(m => m.ProcessingOrdersStatistic)
                .Include(m => m.NewOrdersStatistic)
                .Include(m => m.TotalActivityStatistic)
                .FirstOrDefaultAsync(m => m.Producer == producer
                    && m.ProducerId == producerId
                    && m.StartDate.Year == lastDayOfPreviousMonth.Year
                    && m.StartDate.Month == lastDayOfPreviousMonth.Month
                    && m.StartDate.Day == 1
                    && m.EndDate.Year == lastDayOfPreviousMonth.Year
                    && m.EndDate.Month == lastDayOfPreviousMonth.Month
                    && m.EndDate.Day == lastDayOfPreviousMonth.Day);

            return previousStatistic;
        }

        private async Task<float> CalculatePercentageChanges(decimal currentCount, decimal previousCount)
        {
            if(previousCount > 0)
            {
                float curPercent = (float)(currentCount * 100 / previousCount);

                return curPercent - 100f;
            }
            else
            {
                return 100;
            }
        }

        private async Task<bool> ExistsOrdersForPeriodOfTime(Guid producerId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var existsOrdersForPreviousMonth = DbContext.Orders.Any(o => o.CreationDate.Year >= startDate.Year 
                && o.ProducerId == producerId
                && o.CreationDate.Month >= startDate.Month
                && o.CreationDate.Day >= startDate.Day
                && o.CreationDate.Year <= endDate.Year
                && o.CreationDate.Month <= endDate.Month
                && o.CreationDate.Day <= endDate.Day);

            return existsOrdersForPreviousMonth;
        }
    }

}
