using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Domain;
using Microsoft.EntityFrameworkCore;

namespace FarmersMarketplace.Application.Services.Business
{
    public class ProductTrendService
    {
        private readonly int PopularEntitiesCount;
        private readonly IApplicationDbContext DbContext;
        private List<Product> PopularProducts { get; set; }

        public ProductTrendService(IApplicationDbContext dbContext, int popularEntitiesCount)
        {
            DbContext = dbContext;
            PopularEntitiesCount = popularEntitiesCount;
        }

        public async Task<IEnumerable<Product>> UpdateAndGet()
        {
            PopularProducts = await DbContext.Products
                .Select(p => new
                {
                    Product = p,
                    OrderCount = DbContext.OrderItems
                        .Count(oi => oi.ProductId == p.Id)
                })
                .OrderByDescending(p => p.OrderCount)
                .Take(PopularEntitiesCount)
                .OrderByDescending(p => p.Product.Feedbacks.AverageRating)
                .Select(p => p.Product)
                .ToListAsync();

            return PopularProducts;
        }
    }
}
