using Agroforum.Application.DataTransferObjects.Employee;
using Agroforum.Application.ViewModels.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agroforum.Application.Services.Business
{
    public interface IEmployeeService
    {
        Task Add(AddPermissionsDto addPermissionsDto);
        Task Update(UpdatePermissionsDto updatePermissionsDto);
        Task Delete(Guid permissionsId);
        Task<PermissionsVm> Get(Guid permissionsId);
        Task<EmployeeListVm> GetAll(Guid farmId);
        Task<EmployeeListVm> Search(string identifier);
        Task Confirm(Guid permissionsId, Guid accountId);
    }
}
