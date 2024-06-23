using FarmersMarketplace.Domain.Feedbacks;

namespace FarmersMarketplace.Application.Interfaces
{
    public interface IFeedbackSearchSynchronizer
    {
        Task Create(Feedback obj, Guid reviewedEntityId, string reviewedEntityName, FeedbackType reviewedEntity, string reviewedEntityImage);
        Task Update(Feedback obj);
        Task UpdateCustomerData(Guid customerId, string customerName, string customerImage);
        Task Delete(Guid id);
    }
}
