using AutoMapper;
using FarmersMarketplace.Application.DataTransferObjects.Order;
using FarmersMarketplace.Application.DataTransferObjects.Product;
using FarmersMarketplace.Application.ViewModels.Order;
using FarmersMarketplace.Application.ViewModels.Product;
using FarmersMarketplace.Domain;
using FarmersMarketplace.Elasticsearch.Documents;
using Nest;
using Newtonsoft.Json;
using ApplicationException = FarmersMarketplace.Application.Exceptions.ApplicationException;

namespace FarmersMarketplace.Elasticsearch.SearchProviders
{
    public class CustomerProductSearchProvider : BaseSearchProvider<GetCustomerProductListDto, ProductDocument, CustomerProductListVm, CustomerProductAutocompleteDto>
    {
        private readonly IMapper Mapper;

        public CustomerProductSearchProvider(IElasticClient client, IMapper mapper) : base(client)
        {
            SearchDescriptor.Index(Indecies.Products);
            Mapper = mapper;
        }

        public override async Task<List<string>> Autocomplete(CustomerProductAutocompleteDto request)
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
                                    .Boost(4.0)))))
                .Source(so => so
                    .Includes(i => i
                        .Field(f => f.Name))));

            if (!autocompleteResponse.IsValid)
            {
                string message = $"Products documents was not got successfully for autocomplete from Elasticsearch. Request:\n {JsonConvert.SerializeObject(request)}\n Debug information: {autocompleteResponse.DebugInformation}";
                throw new ApplicationException(message, "ProductsNotGotSuccessfully");
            }

            return autocompleteResponse.Documents.Select(d => d.Name).ToList();
        }

        protected override async Task ApplyFilter()
        {
            MustQueries.Add(q => q
                .Bool(b => b
                    .MustNot(mn => mn
                        .Term(t => t
                            .Field(p => p.Status)
                            .Value(ProductStatus.Private)))));

            if (SearchRequest.Filter == null)
                return;

            var filter = SearchRequest.Filter;
            
            if (filter.MaxPrice.HasValue)
            {
                MustQueries.Add(q => q
                    .Range(r => r
                        .Field(fd => fd.PricePerOne)
                        .LessThanOrEquals((double)filter.MaxPrice)));
            }

            if (filter.MinPrice.HasValue)
            {
                MustQueries.Add(q => q
                    .Range(r => r
                        .Field(fd => fd.PricePerOne)
                        .GreaterThanOrEquals((double)filter.MinPrice)));
            }

            if (filter.MaxCount.HasValue)
            {
                MustQueries.Add(q => q
                    .Range(r => r
                        .Field(fd => fd.Count)
                        .LessThanOrEquals(filter.MaxCount)));
            }

            if (filter.MinCount.HasValue)
            {
                MustQueries.Add(q => q
                    .Range(r => r
                        .Field(fd => fd.Count)
                        .GreaterThanOrEquals(filter.MinCount)));
            }

            if (filter.MinCreationDate.HasValue)
            {
                MustQueries.Add(q => q
                    .DateRange(r => r
                        .Field(fd => fd.CreationDate)
                        .GreaterThanOrEquals((DateTime)filter.MinCreationDate)));
            }

            if (filter.MaxCreationDate.HasValue)
            {
                MustQueries.Add(q => q
                    .DateRange(r => r
                        .Field(fd => fd.CreationDate)
                        .LessThanOrEquals((DateTime)filter.MaxCreationDate)));
            }

            if (filter.ReceivingMethods != null && filter.ReceivingMethods.Any())
            {
                MustQueries.Add(q => q
                    .Terms(t => t
                        .Field(fd => fd.ReceivingMethods)
                        .Terms(filter.ReceivingMethods)));
            }

            if (filter.OnlyOnlinePayment == true)
            {
                MustQueries.Add(q => q
                    .Term(t => t
                        .Field(p => p.HasOnlinePayment)
                        .Value(true)));
            }

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
                                    .Field(p => p.ProducerId)
                                    .Terms(filter.Farms)),
                        s => s.Bool(b2 => b2
                            .Must(m => m.Term(t => t
                                .Field(p => p.Producer)
                                .Value(Producer.Seller)),
                                m => m.Terms(t => t
                                    .Field(p => p.ProducerId)
                                    .Terms(filter.Sellers)))))))));
            }
            else if (filter.Farms != null && filter.Farms.Any())
            {
                MustQueries.Add(q => q.Bool(b => b
                    .Must(m => m.Term(t => t
                        .Field(p => p.Producer)
                        .Value(Producer.Farm)),
                        m => m.Terms(t => t
                            .Field(p => p.ProducerId)
                            .Terms(filter.Farms)))));
            }
            else if(filter.Sellers != null && filter.Sellers.Any())
            {
                MustQueries.Add(q => q.Bool(b => b
                    .Must(m => m.Term(t => t
                        .Field(p => p.Producer)
                        .Value(Producer.Seller)),
                        m => m.Terms(t => t
                            .Field(p => p.ProducerId)
                            .Terms(filter.Sellers)))));
            }

            if (filter.Subcategories != null && filter.Subcategories.Any())
            {
                MustQueries.Add(q => q
                    .Terms(t => t
                        .Field(p => p.SubcategoryId)
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
                                .Boost(1.0)),
                            sh => sh
                            .QueryString(m => m
                                .Fields(f => f.Field(p => p.ArticleNumber))
                                .Query(SearchRequest.Query)
                                .DefaultOperator(Operator.And)
                                .Fuzziness(Fuzziness.Auto)), 
                            sh => sh
                                .Prefix(p => p
                                    .Field(f => f.Name)
                                    .Value(SearchRequest.Query)
                                    .Boost(4.0)))));
            }
        }

        protected override async Task<CustomerProductListVm> Execute()
        {
            var searchResponse = Client.Search<ProductDocument>(SearchDescriptor);

            if (!searchResponse.IsValid)
            {
                string message = $"Products documents was not got successfully from Elasticsearch. Request:\n {JsonConvert.SerializeObject(SearchRequest)}\n Debug information: {searchResponse.DebugInformation}";
                throw new ApplicationException(message, "ProductsNotGotSuccessfully");
            }

            var response = new CustomerProductListVm
            {
                Products = new List<CustomerProductLookupVm>(searchResponse.Documents.Count),
            };

            var productList = searchResponse.Documents.ToArray();

            for (int i = 0; i < productList.Length; i++)
            {
                response.Products.Add(Mapper.Map<CustomerProductLookupVm>(productList[i]));
            }

            return response;
        }
    }

}
