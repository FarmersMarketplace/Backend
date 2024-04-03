namespace FarmersMarketplace.Application.Interfaces
{
    public interface ISearchProvider<TRequest, TAutocompleteRequest, TRestonse>
    {
        Task<TRestonse> Search(TRequest request);
        Task<List<string>> Autocomplete(TAutocompleteRequest request);
    }
}
