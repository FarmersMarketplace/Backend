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
                SearchDescriptor.Query(q => q.Range(r => r.Field(fd => fd.PricePerOne).LessThanOrEquals(filter.MaxPrice)));
            }

            if (filter.MinPrice.HasValue)
            {
                SearchDescriptor.Query(q => q.Range(r => r.Field(fd => fd.Price).GreaterThanOrEquals(filter.MinPrice)));
            }

            if (filter.MaxCount.HasValue)
            {
                SearchDescriptor.Query(q => q.Range(r => r.Field(fd => fd.Count).LessThanOrEquals(filter.MaxCount)));
            }

            if (filter.MinCount.HasValue)
            {
                SearchDescriptor.Query(q => q.Range(r => r.Field(fd => fd.Count).GreaterThanOrEquals(filter.MinCount)));
            }

            if (filter.MaxCreationDate.HasValue)
            {
                SearchDescriptor.Query(q => q.Range(r => r.Field(fd => fd.CreationDate).LessThanOrEquals(filter.MaxCreationDate)));
            }

            if (filter.MinCreationDate.HasValue)
            {
                SearchDescriptor.Query(q => q.Range(r => r.Field(fd => fd.CreationDate).GreaterThanOrEquals(filter.MinCreationDate)));
            }

            if (filter.ReceivingMethods != null && filter.ReceivingMethods.Any())
            {
                SearchDescriptor.Query(q => q.Terms(t => t.Field(fd => fd.ReceivingMethod).Terms(filter.ReceivingMethods)));
            }

            if (filter.OnlyOnlinePayment.HasValue)
            {
                SearchDescriptor.Query(q => q.Term(t => t.Field(fd => fd.OnlyOnlinePayment).Value(filter.OnlyOnlinePayment)));
            }
            if (Request.Filter != null)
            {

                if (filter.MinCreationDate.HasValue || filter.MaxCreationDate.HasValue)
                {
                    var dateRangeDescriptor = new DateRangeQueryDescriptor<ProductDocument>()
                        .Field(fd => fd.CreationDate);

                    if (filter.MinCreationDate.HasValue)
                    {
                        dateRangeDescriptor = dateRangeDescriptor.GreaterThanOrEquals(filter.MinCreationDate);
                    }

                    if (filter.MaxCreationDate.HasValue)
                    {
                        dateRangeDescriptor = dateRangeDescriptor.LessThanOrEquals(filter.MaxCreationDate);
                    }

                    SearchDescriptor.Query(q => q.DateRange(dr => dateRangeDescriptor));
                }
            }
        }


        protected override async Task ApplyPagination()
        {
            SearchDescriptor.Size(Request.CountPerPage)
                       .From((Request.Page - 1) * Request.CountPerPage);
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
