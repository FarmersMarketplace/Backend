using StackExchange.Redis;
using Order = FarmersMarketplace.Domain.Orders.Order;

namespace FarmersMarketplace.Cache.Providers
{
    public class OrderCacheProvider : CacheProvider<Order>
    {
        public OrderCacheProvider(IConnectionMultiplexer redis) : base(redis)
        {
        }

        protected override Guid GetId(Order obj)
        {
            return obj.Id;
        }

        protected override string Key(Guid id)
        {
            return $"order-{id}";
        }
    }
}
