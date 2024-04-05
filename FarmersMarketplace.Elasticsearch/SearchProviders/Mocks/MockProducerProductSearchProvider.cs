using FarmersMarketplace.Application.DataTransferObjects.Product;
using FarmersMarketplace.Application.Filters;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Application.ViewModels.Product;
using FarmersMarketplace.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FarmersMarketplace.Elasticsearch.SearchProviders.Mocks
{
    public class MockProducerProductSearchProvider : ISearchProvider<GetProducerProductListDto, ProducerProductListVm, ProducerProductAutocompleteDto>
    {
        private readonly IApplicationDbContext DbContext;

        public MockProducerProductSearchProvider(IApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<List<string>> Autocomplete(ProducerProductAutocompleteDto request)
        {
            return await DbContext.Products.Where(p => p
                    .Name.Contains(request.Query)
                    && p.Producer == request.Producer
                    && p.ProducerId == request.ProducerId)
                .Take(request.Count)
                .Select(p => p.Name)
                .ToListAsync();
        }

        public async Task<ProducerProductListVm> Search(GetProducerProductListDto request)
        {
            var products = DbContext.Products.Include(p => p.Category)
                .Include(p => p.Subcategory)
                .Where(p => p.Producer == request.Producer
                    && p.ProducerId == request.ProducerId)
                .ToList();

            if (request.Filter != null)
            {
                products = await ApplyFilter(request.Filter, products);
            }

            if (!request.Query.IsNullOrEmpty())
            {
                products = products.Where(p => p.Name.Contains(request.Query)).ToList();
            }

            products = products.Skip((request.Page - 1) * request.PageSize)
               .Take(request.PageSize)
               .ToList();

            var vm = new ProducerProductListVm 
            { 
                Products = new List<ProducerProductLookupVm>(),
                Count = products.Count
            };

            for (int i = 0; i < products.Count; i++)
            {
                vm.Products.Add(new ProducerProductLookupVm
                {
                    Id = products[i].Id,
                    Name = products[i].Name,
                    ArticleNumber = products[i].ArticleNumber,
                    Category = products[i].Category.Name,
                    Subcategory = products[i].Subcategory.Name,
                    Rest = products[i].Count,
                    UnitOfMeasurement = products[i].UnitOfMeasurement,
                    PricePerOne = products[i].PricePerOne,
                    CreationDate = products[i].CreationDate,
                    Status = products[i].Status,
                });
            }

            return vm;
        }

        private async Task<List<Product>> ApplyFilter(ProducerProductFilter filter, List<Product> products)
        {
            if (filter.Statuses != null && filter.Statuses.Any())
            {
                products = products.Where(p => filter.Statuses.Contains(p.Status)).ToList();
            }

            if (filter.Subcategories != null && filter.Subcategories.Any())
            {
                products = products.Where(p => filter.Subcategories.Contains(p.SubcategoryId)).ToList();
            }

            if (filter.MaxRest.HasValue)
            {
                products = products.Where(p => p.Count <= filter.MaxRest).ToList();
            }

            if (filter.MinRest.HasValue)
            {
                products = products.Where(p => p.Count >= filter.MinRest).ToList();
            }

            if (filter.MinCreationDate.HasValue)
            {
                products = products.Where(p => p.CreationDate >= filter.MinCreationDate).ToList();
            }

            if (filter.MaxCreationDate.HasValue)
            {
                products = products.Where(p => p.CreationDate <= filter.MaxCreationDate).ToList();
            }

            if (filter.UnitsOfMeasurement != null && filter.UnitsOfMeasurement.Any())
            {
                products = products.Where(p => filter.UnitsOfMeasurement.Contains(p.UnitOfMeasurement)).ToList();
            }

            return products;
        }
    }

}
