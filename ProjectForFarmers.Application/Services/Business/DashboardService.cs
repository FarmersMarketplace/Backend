using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectForFarmers.Application.Exceptions;
using ProjectForFarmers.Application.Helpers;
using ProjectForFarmers.Application.Interfaces;
using ProjectForFarmers.Application.ViewModels.Order;
using ProjectForFarmers.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.Services.Business
{
    public class DashboardService : Service, IDashboardService
    {
        private readonly StatisticService StatisticService;

        public DashboardService(IMapper mapper, IApplicationDbContext dbContext, IConfiguration configuration) : base(mapper, dbContext, configuration)
        {
            StatisticService = new StatisticService(dbContext);
        }

        public async Task<DashboardVm> Get(Guid id)
        {
            var monthStatistic = await DbContext.MonthesStatistics.FirstOrDefaultAsync(s => s.Id == id);

            if (monthStatistic == null)
            {
                string message = $"Statistic for month with id {id} was not found.";
                string userFacingMessage = CultureHelper.Exception("StatisticWithIdNotExist", id.ToString());

                throw new NotFoundException(message, userFacingMessage);
            }

            var vm = Mapper.Map<DashboardVm>(monthStatistic);

            return vm;
        }

        public async Task<DashboardVm> GetCurrentMonth(Guid producerId, Producer producer)
        {
            var currentMonthStatistic = await StatisticService.CalculateStatisticForMonth(producerId, producer, DateTimeOffset.Now);

            var currentMonthDashboardVm = Mapper.Map<DashboardVm>(currentMonthStatistic);

            if (currentMonthStatistic.CustomerWithHighestPaymentId == null)
            {
                currentMonthDashboardVm.CustomerWithHighestPaymentName = "Customer with highest payment was not found";
            }

            var customerWithHighestPayment = await DbContext.Accounts.FirstOrDefaultAsync(a => a.Id == currentMonthStatistic.CustomerWithHighestPaymentId);
            if (customerWithHighestPayment != null)
            {
                currentMonthDashboardVm.CustomerWithHighestPaymentName = $"{customerWithHighestPayment.Surname} {customerWithHighestPayment.Name}";
            }
            else
            {
                currentMonthDashboardVm.CustomerWithHighestPaymentName = "Customer with highest payment was not found";
            }

            return currentMonthDashboardVm;
        }

        public async Task<LoadDashboardVm> Load(Guid producerId, Producer producer)
        {
            var statistics = DbContext.MonthesStatistics.Where(m => m.ProducerId == producerId
                 && m.Producer == producer).ToList();

            var currentMonthStatistic = await StatisticService.CalculateStatisticForMonth(producerId, producer, DateTimeOffset.Now);
            statistics.Add(currentMonthStatistic);

            var statisticsVm = statistics.Select(s => new MonthStatisticLookupVm
            {
                Id = s.Id,
                StartDate = s.StartDate,
                EndDate = s.EndDate
            }).ToList();

            var currentMonthDashboardVm = Mapper.Map<DashboardVm>(currentMonthStatistic);
            var customerWithHighestPayment = await DbContext.Accounts.FirstOrDefaultAsync(a => a.Id == currentMonthStatistic.CustomerWithHighestPaymentId);

            if (customerWithHighestPayment != null)
            {
                currentMonthDashboardVm.CustomerWithHighestPaymentName = $"{customerWithHighestPayment.Surname} {customerWithHighestPayment.Name}";
            }
            else
            {
                currentMonthDashboardVm.CustomerWithHighestPaymentName = "Customer with highest payment was not found";
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
