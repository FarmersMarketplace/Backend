using FarmersMarketplace.Application.Interfaces;
using Nest;

namespace FarmersMarketplace.Elasticsearch.SearchProviders
{
    public abstract class SearchProvider<TRequest, TDocument, TResponse> : ISearchProvider<TRequest, TResponse> where TDocument : class
    {
        protected readonly IElasticClient Client;
        protected SearchDescriptor<TDocument> SearchDescriptor { get; set; }
        protected TRequest Request { get; set; }

        public SearchProvider(IElasticClient client)
        {
            Client = client;
            SearchDescriptor = new SearchDescriptor<TDocument>();
        }

        public async Task<TResponse> Search(TRequest request)
        {
            Request = request;

            await ApplyQuery();
            await ApplyFilter();
            await ApplySorting();
            await ApplyPagination();

            return await Execute();
        }

        protected abstract Task ApplyQuery();
        protected abstract Task ApplyFilter();
        protected abstract Task ApplySorting();
        protected abstract Task ApplyPagination();
        protected abstract Task<TResponse> Execute();
    }

}
