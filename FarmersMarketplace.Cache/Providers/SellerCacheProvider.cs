using FarmersMarketplace.Domain.Accounts;
using StackExchange.Redis;

namespace FarmersMarketplace.Cache.Providers
{
    public class SellerCacheProvider : BaseCacheProvider<Seller>
    {
        public SellerCacheProvider(IConnectionMultiplexer redis) : base(redis)
        {
        }

        protected override Guid GetId(Seller obj)
        {
            return obj.Id;
        }

        protected override string Key(Guid id)
        {
            return $"seller-{id}";
        }
    }
}
