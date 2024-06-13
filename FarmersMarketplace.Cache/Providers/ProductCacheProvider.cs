using FarmersMarketplace.Domain;
using StackExchange.Redis;

namespace FarmersMarketplace.Cache.Providers
{
    public class ProductCacheProvider : CacheProvider<Product>
    {
        public ProductCacheProvider(IConnectionMultiplexer redis) : base(redis)
        {
        }

        protected override Guid GetId(Product obj)
        {
            return obj.Id;
        }

        protected override string Key(Guid id)
        {
            return $"product-{id}";
        }
    }
}
