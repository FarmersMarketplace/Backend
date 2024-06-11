using AutoMapper;
using FarmersMarketplace.Application.DataTransferObjects.Producers;
using FarmersMarketplace.Application.ViewModels.Producers;
using FarmersMarketplace.Domain;
using FarmersMarketplace.Elasticsearch.Documents;
using Nest;
using Newtonsoft.Json;
using ApplicationException = FarmersMarketplace.Application.Exceptions.ApplicationException;

namespace FarmersMarketplace.Elasticsearch.SearchProviders
{
    public class MapSearchProvider : SearchProvider<GetProducerMarkersDto, ProducerDocument, ProducerMarkerListVm, ProducerAutocompleteDto>
    {
        private readonly IMapper Mapper;

        public MapSearchProvider(IElasticClient client, IMapper mapper) : base(client)
        {
            SearchDescriptor.Index(Indecies.Producers);
            Mapper = mapper;
        }

        public override async Task<List<string>> Autocomplete(ProducerAutocompleteDto request)
        {
            var autocompleteResponse = await Client.SearchAsync<ProducerDocument>(s => s
                .Index(Indecies.Producers)
                .Size(request.Count)
                .Query(q => q
                    .Bool(b => b
                        .Should(sh => sh
                            .QueryString(qs => qs
                                .Fields(f => f.Field(p => p.Name))
                                .Query($"{request.Query}~")
                                .DefaultOperator(Operator.And)
                                .Boost(1.0)),
                            sh => sh
                                .Wildcard(w => w
                                    .Field(f => f.Name)
                                    .Boost(2.0)
                                    .Value($"*{request.Query}*")),
                            sh => sh
                                .Prefix(p => p
                                    .Field(f => f.Name)
                                    .Value(request.Query)
                                    .Boost(4.0)))))
                .Source(so => so
                    .Includes(i => i
                        .Field(f => f.Name))));

            if (!autocompleteResponse.IsValid)
            {
                string message = $"Producers documents was not got successfully for autocomplete from Elasticsearch. Request:\n {JsonConvert.SerializeObject(request)}\n Debug information: {autocompleteResponse.DebugInformation}";
                throw new ApplicationException(message, "ProducersNotGotSuccessfully");
            }

            return autocompleteResponse.Documents.Select(d => d.Name).ToList();
        }

        protected override async Task ApplyFilter()
        {
            if (SearchRequest.Filter == null)
                return;

            var filter = SearchRequest.Filter;

            if (filter.Producer.HasValue)
            {
                MustQueries.Add(q => q
                    .Term(t => t
                        .Field(p => p.Producer)
                        .Value(filter.Producer)));
            }

            if ((filter.Farms != null && filter.Farms.Any())
                && (filter.Sellers != null && filter.Sellers.Any()))
            {
                MustQueries.Add(q => q.Bool(b => b
                    .Should(
                        s => s.Bool(b2 => b2
                            .Must(m => m.Term(t => t
                                .Field(p => p.Producer)
                                .Value(Producer.Farm)),
                                m => m.Terms(t => t
                                    .Field(p => p.Id)
                                    .Terms(filter.Farms)),
                        s => s.Bool(b2 => b2
                            .Must(m => m.Term(t => t
                                .Field(p => p.Producer)
                                .Value(Producer.Seller)),
                                m => m.Terms(t => t
                                    .Field(p => p.Id)
                                    .Terms(filter.Sellers)))))))));
            }
            else if (filter.Farms != null && filter.Farms.Any())
            {
                MustQueries.Add(q => q.Bool(b => b
                    .Must(m => m.Term(t => t
                        .Field(p => p.Producer)
                        .Value(Producer.Farm)),
                        m => m.Terms(t => t
                            .Field(p => p.Id)
                            .Terms(filter.Farms)))));
            }
            else if (filter.Sellers != null && filter.Sellers.Any())
            {
                MustQueries.Add(q => q.Bool(b => b
                    .Must(m => m.Term(t => t
                        .Field(p => p.Producer)
                        .Value(Producer.Seller)),
                        m => m.Terms(t => t
                            .Field(p => p.Id)
                            .Terms(filter.Sellers)))));
            }

            if (filter.Subcategories != null && filter.Subcategories.Any())
            {
                MustQueries.Add(q => q
                    .Terms(t => t
                        .Field(p => p.Subcategories)
                        .Terms(filter.Subcategories)));
            }

            if (!string.IsNullOrEmpty(filter.Region))
            {
                MustQueries.Add(q => q
                    .Term(t => t
                        .Field(p => p.Region)
                        .Value(filter.Region)));
            }
        }

        protected override async Task ApplyPagination()
        {
            MustQueries.Add(q => q
                    .Range(r => r
                        .Field(fd => fd.Latitude)
                        .LessThanOrEquals(SearchRequest.MaxLatitude)));

            MustQueries.Add(q => q
                    .Range(r => r
                        .Field(fd => fd.Latitude)
                        .GreaterThanOrEquals(SearchRequest.MinLatitude)));

            MustQueries.Add(q => q
                    .Range(r => r
                        .Field(fd => fd.Longitude)
                        .LessThanOrEquals(SearchRequest.MaxLongitude)));

            MustQueries.Add(q => q
                    .Range(r => r
                        .Field(fd => fd.Longitude)
                        .GreaterThanOrEquals(SearchRequest.MinLongitude)));
        }

        protected override async Task ApplyQuery()
        {
            if (!string.IsNullOrEmpty(SearchRequest.Query))
            {
                SearchRequest.Query = SearchRequest.Query.ToLower();

                MustQueries.Add(q => q
                    .Bool(b => b
                        .Should(sh => sh
                            .Wildcard(w => w
                                .Field(f => f.Name)
                                .Boost(2.0)
                                .Value($"*{SearchRequest.Query}*")),
                            sh => sh
                            .QueryString(qs => qs
                                .Fields(f => f.Field(p => p.Name))
                                .Query($"{SearchRequest.Query}~")
                                .DefaultOperator(Operator.And)
                                .Boost(1.0)),
                            sh => sh
                                .Prefix(p => p
                                    .Field(f => f.Name)
                                    .Value(SearchRequest.Query)
                                    .Boost(4.0)))));
            }
        }

        protected override async Task<ProducerMarkerListVm> Execute()
        {
            var searchResponse = Client.Search<ProducerDocument>(SearchDescriptor);

            if (!searchResponse.IsValid)
            {
                string message = $"Producers documents was not got successfully from Elasticsearch. Request:\n {JsonConvert.SerializeObject(SearchRequest)}\n Debug information: {searchResponse.DebugInformation}";
                throw new ApplicationException(message, "ProducersNotGotSuccessfully");
            }

            var response = new ProducerMarkerListVm
            {
                Producers = new List<ProducerMarkerLookupVm>(searchResponse.Documents.Count)
            };

            var productList = searchResponse.Documents.ToArray();

            for (int i = 0; i < productList.Length; i++)
            {
                response.Producers.Add(Mapper.Map<ProducerMarkerLookupVm>(productList[i]));
            }

            return response;
        }
    }
}
