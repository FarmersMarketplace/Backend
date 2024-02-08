using ProjectForFarmers.Application.ViewModels.Order;
using ProjectForFarmers.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.Services.Business
{
    public interface IDashboardService
    {
        public Task<LoadDashboardVm> Load(Guid producerId, Producer producer);
        public Task<DashboardVm> Get(Guid id);
        public Task<DashboardVm> GetCurrentMonth(Guid producerId, Producer producer);

    }
}
