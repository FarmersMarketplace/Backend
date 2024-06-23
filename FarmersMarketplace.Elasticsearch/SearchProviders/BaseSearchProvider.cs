using FarmersMarketplace.Application.Interfaces;
using Nest;

namespace FarmersMarketplace.Elasticsearch.SearchProviders
{
    public abstract class BaseSearchProvider<TRequest, TDocument, TResponse, TAutocompleteRequest> : ISearchProvider<TRequest, TResponse, TAutocompleteRequest> where TDocument : class
    {
        protected readonly IElasticClient Client;
        protected List<Func<QueryContainerDescriptor<TDocument>, QueryContainer>> MustQueries { get; set; }
        protected readonly SearchDescriptor<TDocument> SearchDescriptor;
        protected TRequest SearchRequest { get; set; }

        public BaseSearchProvider(IElasticClient client)
        {
            SearchDescriptor = new SearchDescriptor<TDocument>();
            Client = client;
            MustQueries = new List<Func<QueryContainerDescriptor<TDocument>, QueryContainer>>();
        }

        public async Task<TResponse> Search(TRequest request)
        {
            SearchRequest = request;

            await ApplyQuery();
            await ApplyFilter();
            await ApplySorting();
            await ApplyPagination();

            SearchDescriptor.Query(q => q
                    .Bool(b => b
                        .Must(MustQueries)));

            return await Execute();
        }

        protected abstract Task ApplyQuery();
        protected abstract Task ApplyFilter();

        protected virtual async Task ApplySorting()
        {
            SearchDescriptor.Sort(sort => sort
                .Descending("_score"));
        }

        protected abstract Task ApplyPagination();
        protected abstract Task<TResponse> Execute();

        public abstract Task<List<string>> Autocomplete(TAutocompleteRequest request);
    }

}
