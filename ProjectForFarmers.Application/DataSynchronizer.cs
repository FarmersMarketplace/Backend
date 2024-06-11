using FarmersMarketplace.Application.Interfaces;

namespace FarmersMarketplace.Application
{
    public class DataSynchronizer<T>
    {
        private readonly ICacheSynchronizer<T> CacheSynchronizer;
        private readonly ISearchSynchronizer<T> SearchSynchronizer;

        public DataSynchronizer(ICacheSynchronizer<T> cacheSynchronizer, ISearchSynchronizer<T> searchSynchronizer)
        {
            CacheSynchronizer = cacheSynchronizer;
            SearchSynchronizer = searchSynchronizer;
        }

        public async Task Create(T obj)
        {
            await CacheSynchronizer.Create(obj);
            await SearchSynchronizer.Create(obj);
        }

        public async Task Update(T obj)
        {
            await CacheSynchronizer.Update(obj);
            await SearchSynchronizer.Update(obj);
        }

        public async Task Delete(Guid id)
        {
            await CacheSynchronizer.Delete(id);
            await SearchSynchronizer.Delete(id);
        }
    }
}
