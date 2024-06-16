using AutoMapper;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Domain.Feedbacks;
using FarmersMarketplace.Elasticsearch.Documents;
using Nest;
using ApplicationException = FarmersMarketplace.Application.Exceptions.ApplicationException;

namespace FarmersMarketplace.Elasticsearch.Synchronizers
{
    public class FeedbackSynchronizer : IFeedbackSearchSynchronizer
    {
        private readonly IElasticClient Client;
        private readonly IMapper Mapper;
        private readonly IApplicationDbContext DbContext;

        public FeedbackSynchronizer(IElasticClient client, IMapper mapper, IApplicationDbContext dbContext)
        {
            Client = client;
            Mapper = mapper;
            DbContext = dbContext;
        }

        public async Task Create(Feedback obj, Guid reviewedEntityId, string reviewedEntityName, FeedbackType reviewedEntity, string reviewedEntityImage)
        {
            var document = Mapper.Map<FeedbackDocument>(obj);
            document.ReviewedEntityId = reviewedEntityId;
            document.ReviewedEntityName = reviewedEntityName;
            document.ReviewedEntity = reviewedEntity;
            document.ReviewedEntityImage = reviewedEntityImage;

            await Client.IndexDocumentAsync(document);
        }

        public async Task Delete(Guid id)
        {
            await Client.DeleteAsync<FeedbackDocument>(id);
        }

        public async Task Update(Feedback obj)
        {
            var getResponse = await Client.GetAsync<FeedbackDocument>(obj.Id);

            if (!getResponse.Found)
            {
                throw new ApplicationException($"Feedback with Id {obj.Id} not found.", "UpdateDataError");
            }

            var document = getResponse.Source;

            document.CustomerId = obj.CustomerId;
            document.Comment = obj.Comment;
            document.Rating = obj.Rating;
            document.Date = obj.Date;

            var indexResponse = await Client.IndexDocumentAsync(document);

            if (!indexResponse.IsValid)
            {
                throw new ApplicationException($"Failed to update document: {indexResponse.ServerError.Error.Reason}", "UpdateDataError");
            }
        }

        public async Task UpdateCustomerData(Guid customerId, string customerName, string customerImage)
        {
            var searchResponse = await Client.SearchAsync<FeedbackDocument>(s => s
            .Query(q => q
                .Term(f => f.CustomerId, customerId))
            .Size(10000));

            if (!searchResponse.IsValid)
            {
                throw new ApplicationException($"Failed to search documents: {searchResponse.ServerError.Error.Reason}", "UpdateDataError");
            }

            foreach (var document in searchResponse.Documents)
            {
                document.CustomerName = customerName;
                document.CustomerImage = customerImage;

                var indexResponse = await Client.IndexDocumentAsync(document);

                if (!indexResponse.IsValid)
                {
                    throw new ApplicationException($"Failed to update document: {indexResponse.ServerError.Error.Reason}", "UpdateDataError");
                }
            }
        }
    }
}
