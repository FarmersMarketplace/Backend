using FarmersMarketplace.Application.ViewModels.Subcategory;

namespace FarmersMarketplace.Application.ViewModels.Category
{
    public class CategoriesAndSubcategoriesVm
    {
        public List<CategoryLookupVm> Categories { get; set; }
        public List<SubcategoryVm> Subcategories { get; set; }

        public CategoriesAndSubcategoriesVm()
        {
            Categories = new List<CategoryLookupVm>();
            Subcategories = new List<SubcategoryVm>();
        }
    }

}
