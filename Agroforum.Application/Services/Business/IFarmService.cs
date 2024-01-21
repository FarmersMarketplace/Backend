using Agroforum.Application.DataTransferObjects.Farm;
using Agroforum.Application.ViewModels.Farm;
using Agroforum.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agroforum.Application.Services.Business
{
    public interface IFarmService
    {
        Task<FarmVm> Get(Guid farmId);
        Task Create(CreateFarmDto createFarmDto);
        Task Update(UpdateFarmDto updateFarmDto);
        Task Delete(Guid farmId, Guid ownerId);
        Task<FarmListVm> GetAll(Guid userId);
        Task UpdateFarmImages(UpdateFarmImagesDto updateFarmImagesDto);
    }
}
