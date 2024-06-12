namespace FarmersMarketplace.Application.Interfaces
{
    public interface ICacheProvider<T>
    {
        Task Add(T obj);
        Task Update(T obj);
        Task Delete(Guid id);
        Task<bool> Exists(Guid id);
    }
}
