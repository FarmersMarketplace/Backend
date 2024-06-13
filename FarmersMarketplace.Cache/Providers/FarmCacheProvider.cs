using FarmersMarketplace.Domain;
using StackExchange.Redis;

namespace FarmersMarketplace.Cache.Providers
{
    public class FarmCacheProvider : CacheProvider<Farm>
    {
        public FarmCacheProvider(IConnectionMultiplexer redis) : base(redis)
        {
        }

        protected override Guid GetId(Farm obj)
        {
            return obj.Id;
        }

        protected override string Key(Guid id)
        {
            return $"farm-{id}";
        }
    }
}
