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
    public class ProducerProductSearchProvider : SearchProvider<GetProducerProductListDto, ProductDocument, ProducerProductListVm>
    {
        private readonly IMapper Mapper;

        public ProducerProductSearchProvider(IElasticClient client, IMapper mapper) : base(client)
        {
            Mapper = mapper;
        }

        protected override async Task ApplyFilter()
        {
            var mustQueries = new List<Func<QueryContainerDescriptor<ProductDocument>, QueryContainer>>();

            mustQueries.Add(q => q.Bool(b => b.Must(m => m
                        .Term(t => t.Field(p => p.Producer).Value(Request.Producer)),
                              m => m.Term(t => t.Field(p => p.ProducerId).Value(Request.ProducerId)))));

            if (Request.Filter != null)
            {
                var filter = Request.Filter;

                if (filter.Subcategories.Any())
                {
                    mustQueries.Add(q => q.Terms(t => t.Field(p => p.SubcategoryId).Terms(filter.Subcategories)));
                }

                if (filter.Statuses.Any())
                {
                    mustQueries.Add(q => q.Terms(t => t.Field(p => p.Status).Terms(filter.Statuses)));
                }

                if (filter.MinCreationDate.HasValue)
                {
                    mustQueries.Add(q => q.DateRange(r => r.Field(fd => fd.CreationDate).GreaterThanOrEquals(filter.MinCreationDate)));
                }

                if (filter.MaxCreationDate.HasValue)
                {
                    mustQueries.Add(q => q.DateRange(r => r.Field(fd => fd.CreationDate).LessThanOrEquals(filter.MaxCreationDate)));
                }

                if (filter.UnitsOfMeasurement.Any())
                {
                    mustQueries.Add(q => q.Terms(t => t.Field(p => p.UnitOfMeasurement).Terms(filter.UnitsOfMeasurement)));
                }

                if (filter.MinRest.HasValue)
                {
                    mustQueries.Add(q => q.Range(r => r.Field(fd => fd.PricePerOne).LessThanOrEquals(filter.MinRest)));
                }

                if (filter.MaxRest.HasValue)
                {
                    mustQueries.Add(q => q.Range(r => r.Field(fd => fd.PricePerOne).GreaterThanOrEquals(filter.MaxRest)));
                }
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

        protected override async Task<ProducerProductListVm> Execute()
        {
            var searchResponse = Client.Search<ProductDocument>(SearchDescriptor);

            if (!searchResponse.IsValid)
            {
                string message = $"Products documents was not got successfully from Elasticsearch. Request:\n {JsonConvert.SerializeObject(Request)}";
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
                response.Products[i] = Mapper.Map<ProducerProductLookupVm>(productList[i]);
            }

            return response;
        }
    }

}
