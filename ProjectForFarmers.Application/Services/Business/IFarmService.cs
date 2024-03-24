using FarmersMarketplace.Application.DataTransferObjects.Farm;
using FarmersMarketplace.Application.ViewModels.Farm;

namespace FarmersMarketplace.Application.Services.Business
{
    public interface IFarmService
    {
        Task<FarmVm> Get(Guid farmId);
        Task Create(CreateFarmDto dto);
        Task Update(UpdateFarmDto dto, Guid ownerId);
        Task UpdatePaymentData(UpdateFarmPaymentDataDto dto, Guid ownerId);
        Task Delete(Guid farmId, Guid ownerId);
        Task<FarmListVm> GetAll(Guid userId);
        Task UpdateFarmCategoriesAndSubcategories(UpdateFarmCategoriesAndSubcategoriesDto dto, Guid ownerId);
    }
}
