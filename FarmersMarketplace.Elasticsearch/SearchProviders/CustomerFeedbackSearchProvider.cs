using AutoMapper;
using FarmersMarketplace.Application.DataTransferObjects.Feedback;
using FarmersMarketplace.Application.ViewModels.Feedback;
using FarmersMarketplace.Elasticsearch.Documents;
using Nest;
using Newtonsoft.Json;
using ApplicationException = FarmersMarketplace.Application.Exceptions.ApplicationException;

namespace FarmersMarketplace.Elasticsearch.SearchProviders
{
    public class CustomerFeedbackSearchProvider : BaseSearchProvider<GetCustomerFeedbackListDto, FeedbackDocument, CustomerFeedbackListVm, object>
    {
        private readonly IMapper Mapper;

        public CustomerFeedbackSearchProvider(IElasticClient client, IMapper mapper) : base(client)
        {
            Mapper = mapper;
            SearchDescriptor.Index(Indecies.Feedbacks);
        }

        public override Task<List<string>> Autocomplete(object request)
        {
            throw new NotImplementedException();
        }

        protected override async Task ApplyFilter()
        {
            MustQueries.Add(q => q
                .Bool(b => b
                    .Must(m => m
                        .Term(t => t
                            .Field(p => p.CustomerId)
                            .Value(SearchRequest.CustomerId)))));
        }

        protected override async Task ApplyPagination()
        {
            SearchDescriptor.Size(SearchRequest.PageSize)
                       .From((SearchRequest.Page - 1) * SearchRequest.PageSize);
        }

        protected override async Task ApplyQuery()
        {
        }

        protected override async Task ApplySorting()
        {
            SearchDescriptor.Sort(sort => sort
                .Descending(f => f.Date));
        }

        protected override async Task<CustomerFeedbackListVm> Execute()
        {
            var searchResponse = Client.Search<FeedbackDocument>(SearchDescriptor);

            if (!searchResponse.IsValid)
            {
                string message = $"Feedbacks documents was not got successfully from Elasticsearch. Request:\n {JsonConvert.SerializeObject(SearchRequest)}\n Debug information: {searchResponse.DebugInformation}";

                throw new ApplicationException(message, "FeedbacksNotGotSuccessfully");
            }

            var response = new CustomerFeedbackListVm
            {
                Feedbacks = new List<FeedbackForCustomerVm>(),
            };

            var feedbacksList = searchResponse.Documents.ToArray();

            for (int i = 0; i < feedbacksList.Length; i++)
            {
                response.Feedbacks.Add(Mapper.Map<FeedbackForCustomerVm>(feedbacksList[i]));
            }

            return response;
        }
    }
}
