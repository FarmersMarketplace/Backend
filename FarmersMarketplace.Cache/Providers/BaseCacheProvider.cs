using FarmersMarketplace.Application.Interfaces;
using StackExchange.Redis;
using System.Text.Json;
using ApplicationException = FarmersMarketplace.Application.Exceptions.ApplicationException;

namespace FarmersMarketplace.Cache.Providers
{
    public abstract class BaseCacheProvider<T> : ICacheProvider<T>
    {
        private readonly IDatabase Database;
        private readonly TimeSpan Lifetime;

        protected BaseCacheProvider(IConnectionMultiplexer redis)
        {
            Database = redis.GetDatabase();
            Lifetime = TimeSpan.FromMinutes(5);
        }

        public async Task Set(T obj)
        {
            Guid id = GetId(obj);
            var key = Key(id);

            if (Exists(id))
            {
                await Database.KeyExpireAsync(key, Lifetime);
            }
            else
            {
                var json = JsonSerializer.Serialize(obj);
                await Database.StringSetAsync(key, json, Lifetime);
            }
        }

        public async Task Delete(Guid id)
        {
            var key = Key(id);

            if (Exists(id))
            {
                await Database.KeyDeleteAsync(key);
            }
        }

        public bool Exists(Guid id)
        {
            var key = Key(id);
            return Database.KeyExists(key);
        }

        public async Task<T> Get(Guid id)
        {
            if (Exists(id))
            {
                var key = Key(id);
                var obj = JsonSerializer.Deserialize<T>(await Database.StringGetAsync(key));
                await Database.KeyExpireAsync(key, Lifetime);
                return obj;
            }

            throw new ApplicationException($"Object of type {typeof(T)} with id {id} was now found in redis.", "ObjectNotFound");
        }

        public async Task Update(T obj)
        {
            Guid id = GetId(obj);

            if (Exists(id))
            {
                await Delete(id);
            }

            await Set(obj);
        }

        protected abstract Guid GetId(T obj);
        protected abstract string Key(Guid id);
    }
}
