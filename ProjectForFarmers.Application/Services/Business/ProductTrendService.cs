using AutoMapper;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Application.ViewModels.Product;
using FarmersMarketplace.Domain;
using Microsoft.EntityFrameworkCore;

namespace FarmersMarketplace.Application.Services.Business
{
    public class ProductTrendService
    {
        private readonly int PopularEntitiesCount;
        private readonly IApplicationDbContext DbContext;
        private readonly IMapper Mapper;
        private List<CustomerProductLookupVm> PopularProducts { get; set; }
        private HashSet<Guid> ProductIds { get; set; }
        private DateTime UpdateDate { get; set; }
        private readonly int UpdateTime;

        public ProductTrendService(IApplicationDbContext dbContext, IMapper mapper, int popularEntitiesCount, int updateTime)
        {
            DbContext = dbContext;
            PopularEntitiesCount = popularEntitiesCount;
            UpdateTime = updateTime;
            UpdateDate = default;
            Mapper = mapper;
        }

        public async Task<CustomerProductListVm> UpdateAndGet()
        {
            if (DateTime.Now - UpdateDate >= TimeSpan.FromMinutes(UpdateTime))
            {
                var popularProducts = await DbContext.Products
                    .Select(p => new
                    {
                        Product = p,
                        OrderCount = DbContext.OrderItems
                            .Count(oi => oi.ProductId == p.Id)
                    })
                    .OrderByDescending(p => p.OrderCount)
                    .Take(PopularEntitiesCount)
                    .OrderByDescending(p => p.Product.Feedbacks.AverageRating)
                    .Select(p => Mapper.Map<CustomerProductLookupVm>(p))
                    .ToListAsync();

                ProductIds = popularProducts.Select(p => p.Id).ToHashSet();
                UpdateDate = DateTime.Now;

            }

            var vm = new CustomerProductListVm
            {
                Products = PopularProducts,
                Count = PopularProducts.Count
            };

            return vm;
        }

        public async Task UpdateIfPopular(Product product)
        {
            if (ProductIds.Contains(product.Id))
            {
                await UpdateAndGet();
            }
        }
    }
}
