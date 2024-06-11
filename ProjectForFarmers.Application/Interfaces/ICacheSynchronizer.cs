namespace FarmersMarketplace.Application.Interfaces
{
    public interface ICacheSynchronizer<T>
    {
        Task Create(T obj);
        Task Update(T obj);
        Task Delete(Guid id);
    }
}
