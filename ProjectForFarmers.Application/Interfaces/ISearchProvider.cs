using FarmersMarketplace.Application.DataTransferObjects.Product;
using FarmersMarketplace.Application.ViewModels.Product;

namespace FarmersMarketplace.Application.Interfaces
{
    public interface ISearchProvider<TRequest, TRestonse>
    {
        Task<TRestonse> Search(TRequest request);
    }
}
