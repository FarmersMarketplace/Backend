using AutoMapper;
using FarmersMarketplace.Application.DataTransferObjects.Order;
using FarmersMarketplace.Application.ViewModels.Order;
using FarmersMarketplace.Elasticsearch.Documents;
using Nest;
using Newtonsoft.Json;
using ApplicationException = FarmersMarketplace.Application.Exceptions.ApplicationException;

namespace FarmersMarketplace.Elasticsearch.SearchProviders
{
    public class CustomerOrderSearchProvider : BaseSearchProvider<GetCustomerOrderListDto, OrderDocument, CustomerOrderListVm, CustomerOrderAutocompleteDto>
    {
        private readonly IMapper Mapper;

        public CustomerOrderSearchProvider(IElasticClient client, IMapper mapper) : base(client)
        {
            SearchDescriptor.Index(Indecies.Orders);
            Mapper = mapper;
        }

        public override async Task<List<string>> Autocomplete(CustomerOrderAutocompleteDto request)
        {
            var autocompleteResponse = await Client.SearchAsync<OrderDocument>(s => s
                .Index(Indecies.Products)
                .Size(request.Count)
                .Query(q => q
                    .Bool(b => b
                        .Must(m => m
                                .Term(t => t
                                .Field(p => p.CustomerId)
                                .Value(request.CustomerId)),
                            m => m.Wildcard(t => t
                                .Field(p => p.Number)
                                .Value($"*{request.Query}*")))))
                .Source(so => so
                   .Includes(i => i
                       .Field(f => f.Number))));

            if (!autocompleteResponse.IsValid)
            {
                string message = $"Orders documents was not got successfully for autocomplete from Elasticsearch. Request:\n {JsonConvert.SerializeObject(request)}\n Debug information: {autocompleteResponse.DebugInformation}";
                throw new ApplicationException(message, "OrdersNotGotSuccessfully");
            }

            return autocompleteResponse.Documents.Select(d => d.Number.ToString()).ToList();
        }

        protected override async Task ApplyFilter()
        {
        }

        protected override async Task ApplyPagination()
        {
            SearchDescriptor.Size(SearchRequest.PageSize)
                       .From((SearchRequest.Page - 1) * SearchRequest.PageSize);
        }

        protected override async Task ApplyQuery()
        {
            if (!string.IsNullOrEmpty(SearchRequest.Query))
            {
                SearchRequest.Query = SearchRequest.Query.ToLower();

                MustQueries.Add(q => q.Bool(b => b
                        .Must(m => m.Term(t => t
                                .Field(p => p.CustomerId)
                                .Value(SearchRequest.CustomerId)),
                            m => m.Wildcard(t => t
                                .Field(p => p.Number)
                                .Value($"*{SearchRequest.Query}*")))));
            }
        }

        protected override async Task<CustomerOrderListVm> Execute()
        {
            var searchResponse = Client.Search<OrderDocument>(SearchDescriptor);

            if (!searchResponse.IsValid)
            {
                string message = $"Orders documents was not got successfully for autocomplete from Elasticsearch. Request:\n {JsonConvert.SerializeObject(SearchRequest)}\n Debug information: {searchResponse.DebugInformation}";
                throw new ApplicationException(message, "OrdersNotGotSuccessfully");
            }

            var response = new CustomerOrderListVm
            {
                Orders = new List<CustomerOrderLookupVm>(searchResponse.Documents.Count),
                Count = searchResponse.Documents.Count
            };

            var orderList = searchResponse.Documents.ToArray();

            for (int i = 0; i < orderList.Length; i++)
            {
                response.Orders.Add(Mapper.Map<CustomerOrderLookupVm>(orderList[i]));
            }

            return response;
        }
    }
}
