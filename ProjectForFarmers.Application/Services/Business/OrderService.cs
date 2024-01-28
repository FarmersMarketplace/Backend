using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectForFarmers.Application.DataTransferObjects.Order;
using ProjectForFarmers.Application.Exceptions;
using ProjectForFarmers.Application.Interfaces;
using ProjectForFarmers.Application.ViewModels.Order;
using ProjectForFarmers.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.Services.Business
{
    public class OrderService : Service, IOrderService
    {
        private readonly StatisticService StatisticService;

        public OrderService(IMapper mapper, IApplicationDbContext dbContext, IConfiguration configuration) : base(mapper, dbContext, configuration)
        {
            StatisticService = new StatisticService(dbContext);
        }

        public async Task<LoadDashboardVm> LoadDashboard(LoadDashboardDto loadDashboardDto)
        {
            var statistics = DbContext.MonthesStatistics.Where(m => m.ProducerId == loadDashboardDto.ProducerId 
                && m.Producer == loadDashboardDto.Producer).ToList();

            var currentMonthStatistic = await StatisticService.CalculateStatisticForMonth(loadDashboardDto.ProducerId, loadDashboardDto.Producer, DateTimeOffset.Now);
            statistics.Add(currentMonthStatistic);

            var statisticsVm = statistics.Select(s => new MonthStatisticLookupVm
            {
                Id = s.Id,
                StartDate = s.StartDate,
                EndDate = s.EndDate
            }).ToList();

            var currentMonthDashboardVm = Mapper.Map<DashboardVm>(currentMonthStatistic);
            var customerWithHighestPayment = await DbContext.Accounts.FirstOrDefaultAsync(a => a.Id == currentMonthStatistic.CustomerWithHighestPaymentId);

            if (customerWithHighestPayment != null) throw new NotFoundException($"Customer with highest payment was not found(account id: {currentMonthStatistic.CustomerWithHighestPaymentId}).");

            currentMonthDashboardVm.CustomerWithHighestPaymentName = $"{customerWithHighestPayment.Surname} {customerWithHighestPayment.Name}";

            var vm = new LoadDashboardVm
            {
                DateRanges = statisticsVm,
                CurrentMonthDashboard = currentMonthDashboardVm
            };

            return vm;
        }

        public async Task<DashboardVm> GetDashboard(DashboardDto dashboardDto)
        {
            var monthStatistic = await DbContext.MonthesStatistics.FirstOrDefaultAsync(s => s.Id == dashboardDto.Id);

            if (monthStatistic == null && dashboardDto.StartDate.Month == DateTime.Now.Month)
                throw new NotFoundException($"Statistic for month with id {dashboardDto.Id} was not found.");

            var vm = Mapper.Map<DashboardVm>(monthStatistic);

            return vm;
        }

        public async Task<DashboardVm> GetCurrentMonthDashboard(CurrentMonthDashboardDto dashboardDto)
        {
            var currentMonthStatistic = await StatisticService.CalculateStatisticForMonth(dashboardDto.ProducerId, dashboardDto.Producer, DateTimeOffset.Now);

            var currentMonthDashboardVm = Mapper.Map<DashboardVm>(currentMonthStatistic);
            var customerWithHighestPayment = await DbContext.Accounts.FirstOrDefaultAsync(a => a.Id == currentMonthStatistic.CustomerWithHighestPaymentId);

            if (customerWithHighestPayment != null) throw new NotFoundException($"Customer with highest payment was not found(account id: {currentMonthStatistic.CustomerWithHighestPaymentId}).");

            currentMonthDashboardVm.CustomerWithHighestPaymentName = $"{customerWithHighestPayment.Surname} {customerWithHighestPayment.Name}";

            return currentMonthDashboardVm;
        }

        
    }

}
