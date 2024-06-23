namespace FarmersMarketplace.Application.Interfaces
{
    public interface ICacheProvider<T>
    {
        Task<T> Get(Guid id);
        Task Set(T obj);
        Task Update(T obj);
        Task Delete(Guid id);
        bool Exists(Guid id);
    }
}
