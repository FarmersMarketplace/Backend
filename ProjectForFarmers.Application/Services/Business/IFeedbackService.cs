using FarmersMarketplace.Application.DataTransferObjects.Feedback;

namespace FarmersMarketplace.Application.Services.Business
{
    public interface IFeedbackService
    {
        Task Create(CreateFeedbackDto dto, Guid customerId);
        Task Update(UpdateFeedbackDto dto, Guid customerId);
        Task Delete(Guid id, Guid customerId);
    }
}
