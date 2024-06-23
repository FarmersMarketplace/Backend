using FarmersMarketplace.Application.DataTransferObjects.Product;
using FarmersMarketplace.Application.Exceptions;
using FarmersMarketplace.Application.Filters;
using FarmersMarketplace.Application.Helpers;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Application.ViewModels.Product;
using FarmersMarketplace.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FarmersMarketplace.Elasticsearch.SearchProviders.Mocks
{
    public class MockCustomerProductSearchProvider : ISearchProvider<GetCustomerProductListDto, CustomerProductListVm, CustomerProductAutocompleteDto>
    {
        private readonly IApplicationDbContext DbContext;

        public MockCustomerProductSearchProvider(IApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<List<string>> Autocomplete(CustomerProductAutocompleteDto request)
        {
            if (!request.Query.IsNullOrEmpty())
            {
                request.Query = request.Query.Trim().ToLower();

                return await DbContext.Products.Where(p => p
                    .Name.ToLower().Contains(request.Query))
                .Take(request.Count)
                .Select(p => p.Name)
                .ToListAsync();
            }

            return new List<string>();
        }

        public async Task<CustomerProductListVm> Search(GetCustomerProductListDto request)
        {
            List<Product> products = DbContext.Products.Where(p => p.Status != ProductStatus.Private).ToList();

            if (request.Filter != null)
            {
                products = await ApplyFilter(request.Filter, products);
            }

            if (!request.Query.IsNullOrEmpty())
            {
                request.Query = request.Query.Trim().ToLower();

                products = products.Where(p => p.Name.ToLower().Contains(request.Query)).ToList();
            }

            products = products.Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var vm = new CustomerProductListVm { Products = new List<CustomerProductLookupVm>(products.Count) };

            for (int i = 0; i < products.Count; i++)
            {
                string producerName = "";
                string producerImageName = "";

                if (products[i].Producer == Producer.Seller)
                {
                    var seller = await DbContext.Sellers.FirstOrDefaultAsync(s => s.Id == products[i].ProducerId);

                    if (seller == null)
                    {
                        string message = $"Account with Id {products[i].ProducerId} was not found.";

                        throw new NotFoundException(message, "AccountNotFound");
                    }

                    producerName = seller.Name + " " + seller.Surname;
                    producerImageName = (seller.ImagesNames != null && seller.ImagesNames.Any())
                        ? seller.ImagesNames[0]
                        : "";
                }
                else if (products[i].Producer == Producer.Farm)
                {
                    var farm = await DbContext.Farms.FirstOrDefaultAsync(f => f.Id == products[i].ProducerId);

                    if (farm == null)
                    {
                        string message = $"Farm with Id {products[i].ProducerId} was not found.";
                        string userFacingMessage = CultureHelper.Exception("FarmNotFound");

                        throw new NotFoundException(message, userFacingMessage);
                    }

                    producerName = farm.Name;
                    producerImageName = (farm.ImagesNames != null && farm.ImagesNames.Any())
                    ? farm.ImagesNames[0]
                        : "";
                }

                vm.Products.Add(new CustomerProductLookupVm
                {
                    Id = products[i].Id,
                    Name = products[i].Name,
                    Producer = products[i].Producer,
                    ProducerImageName = producerImageName,
                    ProducerName = producerName,
                    ImageName = (products[i].ImagesNames != null && products[i].ImagesNames.Any())
                    ? products[i].ImagesNames[0]
                        : "",
                    ExpirationDate = products[i].CreationDate.AddDays(products[i].ExpirationDays),
                    FeedbacksCount = (products[i].Feedbacks != null)
                        ? (uint)products[i].Feedbacks.Count
                        : 0,
                    Rating = products[i].Feedbacks.AverageRating,
                    PricePerOne = products[i].PricePerOne,
                    Status = products[i].Status
                });
            }

            return vm;
        }

        private async Task<List<Product>> ApplyFilter(CustomerProductFilter filter, List<Product> products)
        {
            if (filter.MaxPrice.HasValue)
            {
                products = products.Where(p => p.PricePerOne <= filter.MaxPrice).ToList();
            }

            if (filter.MinPrice.HasValue)
            {
                products = products.Where(p => p.PricePerOne >= filter.MinPrice).ToList();
            }

            if (filter.MaxCount.HasValue)
            {
                products = products.Where(p => p.Count <= filter.MaxCount).ToList();
            }

            if (filter.MinCount.HasValue)
            {
                products = products.Where(p => p.Count >= filter.MinCount).ToList();
            }

            if (filter.MinCreationDate.HasValue)
            {
                products = products.Where(p => p.CreationDate >= filter.MinCreationDate).ToList();
            }

            if (filter.MaxCreationDate.HasValue)
            {
                products = products.Where(p => p.CreationDate <= filter.MaxCreationDate).ToList();
            }

            if (filter.ReceivingMethods != null && filter.ReceivingMethods.Any())
            {
                products = products.Where(p => p.ReceivingMethods
                    .Exists(x => filter.ReceivingMethods
                        .Contains(x)))
                    .ToList();
            }

            if (filter.Producer.HasValue)
            {
                products = products.Where(p => p.Producer == filter.Producer).ToList();
            }

            if (filter.Farms != null && filter.Farms.Any() &&
                filter.Sellers != null && filter.Sellers.Any())
            {
                products = products.Where(p => (p.Producer == Producer.Farm && filter.Farms.Contains(p.ProducerId)) ||
                                          (p.Producer == Producer.Seller && filter.Sellers.Contains(p.ProducerId))).ToList();
            }
            else if (filter.Farms != null && filter.Farms.Any())
            {
                products = products.Where(p => p.Producer == Producer.Farm &&
                                          filter.Farms.Contains(p.ProducerId)).ToList();
            }
            else if (filter.Sellers != null && filter.Sellers.Any())
            {
                products = products.Where(p => p.Producer == Producer.Seller &&
                                          filter.Sellers.Contains(p.ProducerId)).ToList();
            }

            if (filter.Subcategories != null && filter.Subcategories.Any())
            {
                products = products.Where(p => filter.Subcategories.Contains(p.SubcategoryId)).ToList();
            }

            return products;
        }
    }

}
