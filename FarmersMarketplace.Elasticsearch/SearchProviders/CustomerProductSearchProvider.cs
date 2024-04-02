using AutoMapper;
using FarmersMarketplace.Application.DataTransferObjects.Product;
using FarmersMarketplace.Application.Helpers;
using FarmersMarketplace.Application.ViewModels.Product;
using FarmersMarketplace.Domain;
using FarmersMarketplace.Elasticsearch.Documents;
using Microsoft.IdentityModel.Tokens;
using Nest;
using Newtonsoft.Json;
using ApplicationException = FarmersMarketplace.Application.Exceptions.ApplicationException;

namespace FarmersMarketplace.Elasticsearch.SearchProviders
{
    public class CustomerProductSearchProvider : SearchProvider<GetCustomerProductListDto, ProductDocument, CustomerProductListVm>
    {
        private readonly IMapper Mapper;

        public CustomerProductSearchProvider(IElasticClient client, IMapper mapper) : base(client)
        {
            Mapper = mapper;
        }

        protected override async Task ApplyFilter()
        {
            if (Request.Filter == null) 
                return;

            var filter = Request.Filter;

            if (filter.MaxPrice.HasValue)
            {
                SearchDescriptor.Query(q => q
                    .Range(r => r
                        .Field(fd => fd.PricePerOne)
                            .LessThanOrEquals((double)filter.MaxPrice)));
            }

            if (filter.MinPrice.HasValue)
            {
                SearchDescriptor.Query(q => q
                    .Range(r => r.Field(fd => fd.PricePerOne)
                        .GreaterThanOrEquals((double)filter.MinPrice)));
            }

            if (filter.MaxCount.HasValue)
            {
                SearchDescriptor.Query(q => q
                    .Range(r => r
                        .Field(fd => fd.Count)
                            .LessThanOrEquals(filter.MaxCount)));
            }

            if (filter.MinCount.HasValue)
            {
                SearchDescriptor.Query(q => q
                    .Range(r => r
                        .Field(fd => fd.Count)
                            .GreaterThanOrEquals(filter.MinCount)));
            }

            if (filter.MinCreationDate.HasValue)
            {
                SearchDescriptor.Query(q => q
                    .DateRange(r => r
                        .Field(fd => fd.CreationDate)
                            .GreaterThanOrEquals(filter.MinCreationDate)));
            }

            if (filter.MaxCreationDate.HasValue)
            {
                SearchDescriptor.Query(q => q
                    .DateRange(r => r
                        .Field(fd => fd.CreationDate)
                            .LessThanOrEquals(filter.MaxCreationDate)));
            }

            if (filter.ReceivingMethods != null && filter.ReceivingMethods.Any())
            {
                SearchDescriptor.Query(q => q
                    .Terms(t => t
                        .Field(fd => fd.ReceivingMethods)
                            .Terms(filter.ReceivingMethods)));
            }
            
            if(filter.OnlyOnlinePayment == true)
            {
                SearchDescriptor.Query(q => q
                    .Term(t => t
                        .Field(p => p.HasOnlinePayment)
                        .Value(true)));
            }

            if (filter.Producer.HasValue)
            {
                SearchDescriptor.Query(q => q
                    .Term(t => t
                        .Field(p => p.Producer)
                        .Value(filter.Producer)));
            }

            if (filter.Farms.Any())
            {
                SearchDescriptor.Query(q => q
                    .Bool(b => b
                        .Must(m => m
                            .Term(t => t
                                .Field(p => p.Producer)
                                .Value(Producer.Farm)),
                              m => m
                                .Terms(t => t
                                    .Field(p => p.ProducerId)
                                    .Terms(filter.Farms)))));
            }

            if (filter.Sellers.Any())
            {
                SearchDescriptor.Query(q => q
                    .Bool(b => b
                        .Must(m => m
                            .Term(t => t
                                .Field(p => p.Producer)
                                .Value(Producer.Seller)),
                              m => m
                                .Terms(t => t
                                    .Field(p => p.ProducerId)
                                    .Terms(filter.Sellers)))));
            }

            if (filter.Subcategories.Any())
            {
                SearchDescriptor.Query(q => q
                    .Terms(t => t
                        .Field(p => p.SubcategoryId)
                        .Terms(filter.Subcategories)));
            }

            if (!filter.Region.IsNullOrEmpty())
            {
                SearchDescriptor.Query(q => q
                    .Match(m => m
                        .Field(p => p.Region)
                        .Query(filter.Region)));
            }
        }


        protected override async Task ApplyPagination()
        {
            SearchDescriptor.Size(Request.PageSize)
                       .From((Request.Page - 1) * Request.PageSize);
        }

        protected override async Task ApplyQuery()
        {
            if (!string.IsNullOrEmpty(Request.Query))
            {
                SearchDescriptor.Query(q => q
                    .QueryString(qs => qs
                        .Query(Request.Query)));
            }
        }

        protected override async Task ApplySorting()
        {
            SearchDescriptor.Sort(s => s
                .Descending(fd => fd.CreationDate));
        }

        protected override async Task<CustomerProductListVm> Execute()
        {
            var searchResponse = Client.Search<ProductDocument>(SearchDescriptor);

            if (!searchResponse.IsValid)
            {
                string message = $"Products documents was not got successfully from Elasticsearch. Request:\n {JsonConvert.SerializeObject(Request)}";
                string userFacingMessage = CultureHelper.Exception("ProductsNotGotSuccessfully");

                throw new ApplicationException(message, userFacingMessage);
            }

            var response = new CustomerProductListVm
            {
                Products = new List<CustomerProductLookupVm>(searchResponse.Documents.Count)
            };

            var productList = searchResponse.Documents.ToArray();

            for (int i = 0; i < productList.Length; i++)
            {
                response.Products[i] = Mapper.Map<CustomerProductLookupVm>(productList[i]);
            }

            return response;
        }
    }

}
