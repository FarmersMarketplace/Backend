using FarmersMarketplace.Domain.Accounts;
using StackExchange.Redis;

namespace FarmersMarketplace.Cache.Providers
{
    public class CustomerCacheProvider : CacheProvider<Customer>
    {
        public CustomerCacheProvider(IConnectionMultiplexer redis) : base(redis)
        {
        }

        protected override Guid GetId(Customer obj)
        {
            return obj.Id;
        }

        protected override string Key(Guid id)
        {
            return $"customer-{id}";
        }
    }
}
