using ProjectForFarmers.Application.DataTransferObjects.Order;
using ProjectForFarmers.Application.ViewModels.Order;
using ProjectForFarmers.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.Services.Business
{
    public interface IOrderService
    {
        public Task<DashboardVm> Dashboard(DashboardDto dto);
    }

}
