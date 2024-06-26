﻿using AutoMapper;
using FarmersMarketplace.Application.DataTransferObjects.Dashboard;
using FarmersMarketplace.Application.Exceptions;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Application.ViewModels.Dashboard;
using FarmersMarketplace.Application.ViewModels.Order;
using FarmersMarketplace.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace FarmersMarketplace.Application.Services.Business
{
    public class DashboardService : Service, IDashboardService
    {
        private readonly StatisticService StatisticService;
        private readonly IMemoryCache MemoryCache;

        public DashboardService(IMapper mapper, IApplicationDbContext dbContext, IConfiguration configuration, IMemoryCache memoryCache) : base(mapper, dbContext, configuration)
        {
            StatisticService = new StatisticService(dbContext);
            MemoryCache = memoryCache;
        }

        public async Task<OptionListVm> CustomerAutocomplete(Guid producerId, Producer producer, string query, int count)
        {
            var cacheKey = "";
            var vm = new OptionListVm();

            if (MemoryCache.TryGetValue(cacheKey, out List<string> cachedCustomerNames))
            {
                vm.Options = new HashSet<string>(cachedCustomerNames
                   .Where(customer => customer.ToLower().Contains(query.ToLower()))
                   .Take(count)
                   .ToList());

                return vm;
            }

            var customerIdsHashSet = new HashSet<Guid>(await DbContext.Orders
                .Where(o => o.ProducerId == producerId
                && o.Producer == producer)
                .Select(o => o.CustomerId)
                .ToListAsync());

            var customerNames = await DbContext.Customers
                .Where(a => customerIdsHashSet.Contains(a.Id))
                .Select(a => $"{a.Name} {a.Surname}")
                .Distinct()
                .ToListAsync();

            MemoryCache.Set(cacheKey, customerNames, TimeSpan.FromMinutes(20));

            vm.Options = new HashSet<string>(customerNames
                      .Where(customer => customer.ToLower().Contains(query.ToLower()))
                      .Take(count)
                      .ToList());

            return vm;
        }

        public async Task<DashboardVm> Get(Guid id)
        {
            var monthStatistic = await DbContext.MonthesStatistics.FirstOrDefaultAsync(s => s.Id == id);

            if (monthStatistic == null)
            {
                string message = $"Statistic for month with id {id} was not found.";
                throw new NotFoundException(message, "StatisticWithIdNotExist", id.ToString());
            }

            var vm = Mapper.Map<DashboardVm>(monthStatistic);

            vm.CustomerInfo = new CustomerInfoVm
            {
                Payment = monthStatistic.HighestCustomerPayment,
                PaymentPercentage = monthStatistic.HighestCustomerPaymentPercentage,
                Name = ""
            };

            if (monthStatistic.CustomerWithHighestPaymentId != null)
            {
                var customer = await DbContext.Customers.FirstOrDefaultAsync(a => a.Id == monthStatistic.CustomerWithHighestPaymentId);

                if (customer == null)
                {
                    string message = $"Account with Id {monthStatistic.CustomerWithHighestPaymentId} was not found.";
                    throw new NotFoundException(message, "AccountNotFound");
                }

                vm.CustomerInfo.Name = customer.Surname + " " + customer.Surname;
            }

            return vm;
        }

        public async Task<DashboardVm> GetCurrentMonth(Guid producerId, Producer producer)
        {
            var currentMonthStatistic = await StatisticService.CalculateStatisticForMonth(producerId, producer, DateTime.UtcNow);

            var currentMonthDashboardVm = Mapper.Map<DashboardVm>(currentMonthStatistic);

            if (currentMonthStatistic.CustomerWithHighestPaymentId == null)
            {
                currentMonthDashboardVm.CustomerInfo.Name = "Customer with highest payment was not found";
            }

            var customerWithHighestPayment = await DbContext.Customers.FirstOrDefaultAsync(a => a.Id == currentMonthStatistic.CustomerWithHighestPaymentId);
            if (customerWithHighestPayment != null)
            {
                currentMonthDashboardVm.CustomerInfo.Name = $"{customerWithHighestPayment.Surname} {customerWithHighestPayment.Name}";
            }
            else
            {
                currentMonthDashboardVm.CustomerInfo.Name = "Customer with highest payment was not found";
            }

            return currentMonthDashboardVm;
        }

        public async Task<CustomerInfoVm> SearchCustomer(GetCustomerDto dto)
        {
            var vm = await StatisticService.GetCustomerInfo(dto);

            return vm;
        }

        public async Task<LoadDashboardVm> Load(Guid producerId, Producer producer)
        {
            var statistics = DbContext.MonthesStatistics.Where(m => m.ProducerId == producerId
                 && m.Producer == producer).ToList();

            var currentMonthStatistic = await StatisticService.CalculateStatisticForMonth(producerId, producer, DateTime.UtcNow);
            statistics.Add(currentMonthStatistic);

            var statisticsVm = statistics.Select(s => new MonthStatisticLookupVm
            {
                Id = s.Id,
                StartDate = s.StartDate,
                EndDate = s.EndDate
            }).ToList();

            var currentMonthDashboardVm = Mapper.Map<DashboardVm>(currentMonthStatistic);
            var customerWithHighestPayment = await DbContext.Customers.FirstOrDefaultAsync(a => a.Id == currentMonthStatistic.CustomerWithHighestPaymentId);

            if (customerWithHighestPayment != null)
            {
                currentMonthDashboardVm.CustomerInfo.Name = $"{customerWithHighestPayment.Surname} {customerWithHighestPayment.Name}";
            }
            else
            {
                currentMonthDashboardVm.CustomerInfo.Name = "Customer with highest payment was not found";
            }

            var vm = new LoadDashboardVm
            {
                DateRanges = statisticsVm,
                CurrentMonthDashboard = currentMonthDashboardVm
            };

            return vm;
        }
    }

}
