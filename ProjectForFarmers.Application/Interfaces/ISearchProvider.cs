namespace FarmersMarketplace.Application.Interfaces
{
    public interface ISearchProvider<TRequest, TRestonse, TAutocompleteRequest>
    {
        Task<TRestonse> Search(TRequest request);
        Task<List<string>> Autocomplete(TAutocompleteRequest request);
    }
}
