using AutoMapper;
using FarmersMarketplace.Application.DataTransferObjects.Order;
using FarmersMarketplace.Application.ViewModels.Order;
using FarmersMarketplace.Elasticsearch.Documents;
using Nest;
using Newtonsoft.Json;
using ApplicationException = FarmersMarketplace.Application.Exceptions.ApplicationException;

namespace FarmersMarketplace.Elasticsearch.SearchProviders
{
    public class ProducerOrderSearchProvider : SearchProvider<GetProducerOrderListDto, OrderDocument, ProducerOrderListVm, ProducerOrderAutocompleteDto>
    {
        private readonly IMapper Mapper;

        public ProducerOrderSearchProvider(IElasticClient client, IMapper mapper) : base(client)
        {
            SearchDescriptor.Index(Indecies.Orders);
            Mapper = mapper;
        }

        public override async Task<List<string>> Autocomplete(ProducerOrderAutocompleteDto request)
        {
            var autocompleteResponse = await Client.SearchAsync<OrderDocument>(s => s
                .Index(Indecies.Products)
                .Size(request.Count)
                .Query(q => q
                    .Bool(b => b
                        .Must(m => m
                            .Term(t => t
                                .Field(p => p.Producer)
                                .Value(request.Producer)),
                            m => m.Term(t => t
                                .Field(p => p.ProducerId)
                                .Value(request.ProducerId)),
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
            MustQueries.Add(q => q
                .Bool(b => b
                    .Must(m => m
                        .Term(t => t
                            .Field(p => p.Producer)
                            .Value(SearchRequest.Producer)),
                    m => m.Term(t => t
                        .Field(p => p.ProducerId)
                        .Value(SearchRequest.ProducerId)))));

            if (SearchRequest.Filter != null)
            {
                var filter = SearchRequest.Filter;

                if (filter.Statuses != null && filter.Statuses.Any())
                {
                    MustQueries.Add(q => q
                        .Terms(t => t
                            .Field(p => p.Status)
                            .Terms(filter.Statuses)));
                }

                if (filter.StartDate.HasValue)
                {
                    MustQueries.Add(q => q
                        .DateRange(r => r
                            .Field(fd => fd.CreationDate)
                            .GreaterThanOrEquals(filter.StartDate)));
                }

                if (filter.EndDate.HasValue)
                {
                    MustQueries.Add(q => q
                        .DateRange(r => r
                            .Field(fd => fd.CreationDate)
                            .LessThanOrEquals(filter.EndDate)));
                }

                if (filter.PaymentTypes != null && filter.PaymentTypes.Any())
                {
                    MustQueries.Add(q => q
                        .Terms(t => t
                            .Field(p => p.PaymentType)
                            .Terms(filter.PaymentTypes)));
                }

                if (filter.MinimumAmount.HasValue)
                {
                    MustQueries.Add(q => q
                        .Range(r => r
                            .Field(fd => fd.TotalPayment)
                            .GreaterThanOrEquals((double?)filter.MinimumAmount)));
                }

                if (filter.MaximumAmount.HasValue)
                {
                    MustQueries.Add(q => q
                        .Range(r => r
                            .Field(fd => fd.TotalPayment)
                            .LessThanOrEquals((double?)filter.MaximumAmount)));
                }
            }
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
                        .Must(m => m
                            .Term(t => t
                                .Field(p => p.Producer)
                                .Value(SearchRequest.Producer)),
                            m => m.Term(t => t
                                .Field(p => p.ProducerId)
                                .Value(SearchRequest.ProducerId)),
                            m => m.Wildcard(t => t
                                .Field(p => p.Number)
                                .Value($"*{SearchRequest.Query}*")))));
            }
        }

        protected override async Task<ProducerOrderListVm> Execute()
        {
            var searchResponse = Client.Search<OrderDocument>(SearchDescriptor);

            if (!searchResponse.IsValid)
            {
                string message = $"Orders documents was not got successfully for autocomplete from Elasticsearch. Request:\n {JsonConvert.SerializeObject(SearchRequest)}\n Debug information: {searchResponse.DebugInformation}";
                throw new ApplicationException(message, "OrdersNotGotSuccessfully");
            }

            var response = new ProducerOrderListVm
            {
                Orders = new List<ProducerOrderLookupVm>(searchResponse.Documents.Count),
                Count = searchResponse.Documents.Count
            };

            var orderList = searchResponse.Documents.ToArray();

            for (int i = 0; i < orderList.Length; i++)
            {
                response.Orders.Add(Mapper.Map<ProducerOrderLookupVm>(orderList[i]));
            }

            return response;
        }
    }
}
