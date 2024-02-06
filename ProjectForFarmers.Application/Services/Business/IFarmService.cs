using ProjectForFarmers.Application.DataTransferObjects.Farm;
using ProjectForFarmers.Application.ViewModels.Farm;

namespace ProjectForFarmers.Application.Services.Business
{
    public interface IFarmService
    {
        Task<FarmVm> Get(Guid farmId);
        Task Create(CreateFarmDto createFarmDto);
        Task Update(UpdateFarmDto updateFarmDto);
        Task UpdateSettings(UpdateFarmSettingsDto updateFarmSettingsDto);
        Task Delete(Guid farmId, Guid ownerId);
        Task<FarmListVm> GetAll(Guid userId);
        Task UpdateFarmCategoriesAndSubcategories(UpdateFarmCategoriesAndSubcategoriesDto updateFarmCategoriesAndSubcategoriesDto);
    }
}
