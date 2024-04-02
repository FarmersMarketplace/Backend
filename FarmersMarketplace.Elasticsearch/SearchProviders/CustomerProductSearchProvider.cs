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
            var sd = new SearchDescriptor<object>().Index("Products");
            var mustQueries = new List<Func<QueryContainerDescriptor<ProductDocument>, QueryContainer>>();

            if (filter.MaxPrice.HasValue)
            {
                mustQueries.Add(q => q.Range(r => r.Field(fd => fd.PricePerOne)
                    .LessThanOrEquals((double)filter.MaxPrice)));
            }

            if (filter.MinPrice.HasValue)
            {
                mustQueries.Add(q => q.Range(r => r.Field(fd => fd.PricePerOne)
                    .GreaterThanOrEquals((double)filter.MinPrice)));
            }

            if (filter.MaxCount.HasValue)
            {
                mustQueries.Add(q => q.Range(r => r.Field(fd => fd.Count)
                    .LessThanOrEquals(filter.MaxCount)));
            }

            if (filter.MinCount.HasValue)
            {
                mustQueries.Add(q => q.Range(r => r.Field(fd => fd.Count)
                    .GreaterThanOrEquals(filter.MinCount)));
            }

            if (filter.MinCreationDate.HasValue)
            {
                mustQueries.Add(q => q.DateRange(r => r.Field(fd => fd.CreationDate)
                    .GreaterThanOrEquals((DateTime)filter.MinCreationDate)));
            }

            if (filter.MaxCreationDate.HasValue)
            {
                mustQueries.Add(q => q.DateRange(r => r.Field(fd => fd.CreationDate)
                    .LessThanOrEquals((DateTime)filter.MaxCreationDate)));
            }

            if (filter.ReceivingMethods != null && filter.ReceivingMethods.Any())
            {
                mustQueries.Add(q => q.Terms(t => t.Field(fd => fd.ReceivingMethods)
                    .Terms(filter.ReceivingMethods)));
            }

            if (filter.OnlyOnlinePayment == true)
            {
                mustQueries.Add(q => q.Term(t => t.Field(p => p.HasOnlinePayment).Value(true)));
            }

            if (filter.Producer.HasValue)
            {
                mustQueries.Add(q => q.Term(t => t.Field(p => p.Producer).Value(filter.Producer)));
            }

            if (filter.Farms != null && filter.Farms.Any())
            {
                mustQueries.Add(q => q.Bool(b => b.Must(m => m.Term(t => t.Field(p => p.Producer).Value(Producer.Farm)),
                    m => m.Terms(t => t.Field(p => p.ProducerId).Terms(filter.Farms)))));
            }

            if (filter.Sellers != null && filter.Sellers.Any())
            {
                mustQueries.Add(q => q.Bool(b => b.Must(m => m.Term(t => t.Field(p => p.Producer).Value(Producer.Seller)),
                    m => m.Terms(t => t.Field(p => p.ProducerId).Terms(filter.Sellers)))));
            }

            if (filter.Subcategories != null && filter.Subcategories.Any())
            {
                mustQueries.Add(q => q.Terms(t => t.Field(p => p.SubcategoryId).Terms(filter.Subcategories)));
            }

            if (!string.IsNullOrEmpty(filter.Region))
            {
                mustQueries.Add(q => q.Term(t => t.Field(p => p.Region).Value(filter.Region)));
            }

            SearchDescriptor.Query(q => q
                    .Bool(b => b
                        .Must(mustQueries)));
        }


        protected override async Task ApplyPagination()
        {
            SearchDescriptor.Size(Request.PageSize)
                       .From((Request.Page - 1) * Request.PageSize);
        }

        protected override async Task ApplyQuery()
        {
            //if (!string.IsNullOrEmpty(Request.Query))
            //{
            //    SearchDescriptor.Query(q => q
            //        .QueryString(qs => qs
            //            .Query(Request.Query)));
            //}
        }

        protected override async Task ApplySorting()
        {
            //SearchDescriptor.Index(Indecies.Products)
            //    .Sort(s => s
            //        .Descending(fd => fd.CreationDate));
        }

        protected override async Task<CustomerProductListVm> Execute()
        {
            var searchResponse = Client.Search<ProductDocument>(SearchDescriptor.Index(Indecies.Products));

            if (!searchResponse.IsValid)
            {
                string message = $"Products documents was not got successfully from Elasticsearch. Request:\n {JsonConvert.SerializeObject(Request)}";
                string userFacingMessage = CultureHelper.Exception("ProductsNotGotSuccessfully");

                throw new ApplicationException(message, userFacingMessage);
            }

            var response = new CustomerProductListVm
            {
                Products = new CustomerProductLookupVm[searchResponse.Documents.Count]
            };

            var productList = searchResponse.Documents.Select(p => p.PricePerOne).ToArray();

            for (int i = 0; i < productList.Length; i++)
            {
                //response.Products[i] = Mapper.Map<CustomerProductLookupVm>(productList[i]);
                Console.WriteLine(productList[i]);
            }

            return response;
        }
    }

}
