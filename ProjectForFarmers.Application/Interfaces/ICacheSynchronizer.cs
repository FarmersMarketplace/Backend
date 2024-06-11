namespace FarmersMarketplace.Application.Interfaces
{
    public interface ICacheSynchronizer<T>
    {
        Task Update(T obj);
        Task Delete(Guid id);
    }
}
