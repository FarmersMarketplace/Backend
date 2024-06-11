namespace FarmersMarketplace.Application.Interfaces
{
    public interface ISearchSynchronizer<T>
    {
        Task Create(T obj);
        Task Update(T obj);
        Task Delete(Guid id);
    }
}
