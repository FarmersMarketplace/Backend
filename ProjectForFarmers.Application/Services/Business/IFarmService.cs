using ProjectForFarmers.Application.DataTransferObjects.Farm;
using ProjectForFarmers.Application.ViewModels.Farm;
using ProjectForFarmers.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.Services.Business
{
    public interface IFarmService
    {
        Task<FarmVm> Get(Guid farmId);
        Task Create(CreateFarmDto createFarmDto);
        Task Update(UpdateFarmDto updateFarmDto);
        Task Delete(Guid farmId, Guid ownerId);
        Task<FarmListVm> GetAll(Guid userId);
        Task UpdateFarmImages(UpdateFarmImagesDto updateFarmImagesDto);
        Task UpdateFarmCategoriesAndSubcategories(UpdateFarmCategoriesAndSubcategoriesDto updateFarmCategoriesAndSubcategoriesDto);
    }
}
