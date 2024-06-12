using FarmersMarketplace.Application.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace FarmersMarketplace.Cache.Providers
{
    public abstract class CacheProvider<T> : ICacheProvider<T>
    {
        private readonly IDatabase Database;
        private readonly TimeSpan Lifetime = TimeSpan.FromMinutes(5);

        protected CacheProvider(IConnectionMultiplexer redis)
        {
            Database = redis.GetDatabase();
        }

        public async Task Add(T obj)
        {
            Guid id = GetId(obj);
            var key = Key(id);

            if (await Exists(id))
            {
                await Database.KeyExpireAsync(key, Lifetime);
            }
            else
            {
                var json = JsonSerializer.Serialize(obj);
                await Database.StringSetAsync(key, json);
                await Database.StringSetAsync(key, json, Lifetime);
            }
        }

        public async Task Delete(Guid id)
        {
            var key = Key(id);

            if (await Exists(id))
            {
                await Database.KeyDeleteAsync(key);
            }
        }

        public async Task<bool> Exists(Guid id)
        {
            var key = Key(id);
            return await Database.KeyExistsAsync(key);
        }

        public async Task Update(T obj)
        {
            Guid id = GetId(obj);

            if (await Exists(id))
            {
                await Delete(id);
            }

            await Add(obj);
        }

        protected abstract Guid GetId(T obj);
        protected abstract string Key(Guid id);
    }
}
