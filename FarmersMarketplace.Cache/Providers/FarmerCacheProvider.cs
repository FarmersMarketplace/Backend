using FarmersMarketplace.Domain.Accounts;
using StackExchange.Redis;

namespace FarmersMarketplace.Cache.Providers
{
    public class FarmerCacheProvider : BaseCacheProvider<Farmer>
    {
        public FarmerCacheProvider(IConnectionMultiplexer redis) : base(redis)
        {
        }

        protected override Guid GetId(Farmer obj)
        {
            return obj.Id;
        }

        protected override string Key(Guid id)
        {
            return $"farmer-{id}";
        }
    }   
}
