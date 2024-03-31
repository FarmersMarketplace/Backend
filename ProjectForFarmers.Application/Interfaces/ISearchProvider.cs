namespace FarmersMarketplace.Application.Interfaces
{
    public interface ISearchProvider<TRequest, TRestonse>
    {
        Task<TRestonse> Search(TRequest request);
    }
}
