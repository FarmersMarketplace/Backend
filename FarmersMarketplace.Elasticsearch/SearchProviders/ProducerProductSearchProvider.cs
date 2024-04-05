using AutoMapper;
using FarmersMarketplace.Application.DataTransferObjects.Product;
using FarmersMarketplace.Application.Helpers;
using FarmersMarketplace.Application.ViewModels.Product;
using FarmersMarketplace.Elasticsearch.Documents;
using Nest;
using Newtonsoft.Json;
using ApplicationException = FarmersMarketplace.Application.Exceptions.ApplicationException;

namespace FarmersMarketplace.Elasticsearch.SearchProviders
{
    public class ProducerProductSearchProvider : SearchProvider<GetProducerProductListDto, ProductDocument, ProducerProductListVm, ProducerProductAutocompleteDto>
    {
        private readonly IMapper Mapper;

        public ProducerProductSearchProvider(IElasticClient client, IMapper mapper) : base(client)
        {
            Mapper = mapper;
            SearchDescriptor.Index(Indecies.Products);
        }

        public override async Task<List<string>> Autocomplete(ProducerProductAutocompleteDto request)
        {
            var autocompleteResponse = await Client.SearchAsync<ProductDocument>(s => s
               .Index(Indecies.Products)
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
                                   .Boost(4.0)))
                       .Must(m => m
                        .Term(t => t
                            .Field(p => p.Producer)
                            .Value(request.Producer)),
                        m => m.Term(t => t
                            .Field(p => p.ProducerId)
                            .Value(request.ProducerId)))))
               .Source(so => so
                   .Includes(i => i
                       .Field(f => f.Name))));

            if (!autocompleteResponse.IsValid)
            {
                string message = $"Products documents was not got successfully for autocomplete from Elasticsearch. Request:\n {JsonConvert.SerializeObject(request)}\n Debug information: {autocompleteResponse.DebugInformation}";
                string userFacingMessage = CultureHelper.Exception("ProductsNotGotSuccessfully");

                throw new ApplicationException(message, userFacingMessage);
            }

            return autocompleteResponse.Documents.Select(d => d.Name).ToList();
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

                if (filter.Subcategories.Any())
                {
                    MustQueries.Add(q => q
                        .Terms(t => t
                            .Field(p => p.SubcategoryId)
                            .Terms(filter.Subcategories)));
                }

                if (filter.Statuses.Any())
                {
                    MustQueries.Add(q => q
                        .Terms(t => t
                            .Field(p => p.Status)
                            .Terms(filter.Statuses)));
                }

                if (filter.MinCreationDate.HasValue)
                {
                    MustQueries.Add(q => q
                        .DateRange(r => r
                            .Field(fd => fd.CreationDate)
                            .GreaterThanOrEquals(filter.MinCreationDate)));
                }

                if (filter.MaxCreationDate.HasValue)
                {
                    MustQueries.Add(q => q
                        .DateRange(r => r
                            .Field(fd => fd.CreationDate)
                            .LessThanOrEquals(filter.MaxCreationDate)));
                }

                if (filter.UnitsOfMeasurement.Any())
                {
                    MustQueries.Add(q => q
                        .Terms(t => t
                            .Field(p => p.UnitOfMeasurement)
                            .Terms(filter.UnitsOfMeasurement)));
                }

                if (filter.MinRest.HasValue)
                {
                    MustQueries.Add(q => q
                        .Range(r => r
                            .Field(fd => fd.PricePerOne)
                            .LessThanOrEquals(filter.MinRest)));
                }

                if (filter.MaxRest.HasValue)
                {
                    MustQueries.Add(q => q
                        .Range(r => r
                            .Field(fd => fd.PricePerOne)
                            .GreaterThanOrEquals(filter.MaxRest)));
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
                                .Boost(2.0)),
                            sh => sh
                            .QueryString(m => m
                                .Fields(f => f.Field(p => p.ArticleNumber))
                                .Query(SearchRequest.Query)
                                .DefaultOperator(Operator.And)
                                .Fuzziness(Fuzziness.Auto)))));
            }
        }

        protected override async Task ApplySorting()
        {
            SearchDescriptor.Sort(sort => sort
                .Descending("_score"));
        }

        protected override async Task<ProducerProductListVm> Execute()
        {
            var searchResponse = Client.Search<ProductDocument>(SearchDescriptor);

            if (!searchResponse.IsValid)
            {
                string message = $"Products documents was not got successfully from Elasticsearch. Request:\n {JsonConvert.SerializeObject(SearchRequest)}\n Debug information: {searchResponse.DebugInformation}";
                string userFacingMessage = CultureHelper.Exception("ProductsNotGotSuccessfully");

                throw new ApplicationException(message, userFacingMessage);
            }

            var response = new ProducerProductListVm
            {
                Products = new List<ProducerProductLookupVm>(searchResponse.Documents.Count),
                Count = searchResponse.Documents.Count
            };

            var productList = searchResponse.Documents.ToArray();

            for (int i = 0; i < productList.Length; i++)
            {
                response.Products.Add(Mapper.Map<ProducerProductLookupVm>(productList[i]));
            }

            return response;
        }
    }

}
