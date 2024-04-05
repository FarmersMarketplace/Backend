using FarmersMarketplace.Application.DataTransferObjects;
using FarmersMarketplace.Application.DataTransferObjects.Farm;
using FarmersMarketplace.Application.ViewModels.Farm;

namespace FarmersMarketplace.Application.Services.Business
{
    public interface IFarmService
    {
        Task<FarmVm> GetForProducer(Guid farmId);
        Task Create(CreateFarmDto dto);
        Task Update(UpdateFarmDto dto, Guid ownerId);
        Task UpdatePaymentData(FarmPaymentDataDto dto, Guid ownerId);
        Task Delete(Guid farmId, Guid ownerId);
        Task<FarmListVm> GetAllForProducer(Guid account);
        Task UpdateFarmCategoriesAndSubcategories(UpdateFarmCategoriesAndSubcategoriesDto dto, Guid ownerId);
        Task<CardDataVm> CopyOwnerCardData(Guid ownerId);
        Task<AccountNumberDataVm> CopyOwnerAccountNumberData(Guid ownerId);
    }
}
