using Agroforum.Application.DataTransferObjects.Farm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agroforum.Application.Services.Business
{
    public interface IFarmService
    {
        Task Create(CreateFarmDto createFarmDto);
        Task Update(UpdateFarmDto UpdateFarmDto);
    }
}
