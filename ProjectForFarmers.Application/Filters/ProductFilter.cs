using ProjectForFarmers.Application.ViewModels.Product;
using ProjectForFarmers.Domain;

namespace ProjectForFarmers.Application.Filters
{
    public class ProductFilter : IFilter<List<Product>>
    {
        public List<Guid>? CategoryIds { get; set; }
        public List<Guid>? SubcategoryIds { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<string>? UnitsOfMeasurement { get; set; }
        public uint? MinRest {  get; set; }
        public uint? MaxRest { get; set; }

        public async Task Filter(List<Product> collection)
        {
            collection = collection
                .Where(p =>
                    (IsCategoryValid(p.CategoryId)) &&
                    (IsSubcategoryValid(p.SubcategoryId)) &&
                    (IsStartDateValid(p.CreationDate)) &&
                    (IsEndDateValid(p.CreationDate)) &&
                    (IsUnitsOfMeasurementValid(p.UnitOfMeasurement)) &&
                    (IsMinRestValid(p.Count)) &&
                    (IsMaxRestValid(p.Count)))
                .ToList();
        }

        private bool IsCategoryValid(Guid productCategoryId) => CategoryIds == null || !CategoryIds.Any() || CategoryIds.Contains(productCategoryId);
        private bool IsSubcategoryValid(Guid productSubcategoryId) => SubcategoryIds == null || !SubcategoryIds.Any() || SubcategoryIds.Contains(productSubcategoryId);
        private bool IsStartDateValid(DateTime productCreationDate) => !StartDate.HasValue || productCreationDate >= StartDate;
        private bool IsEndDateValid(DateTime productCreationDate) => !EndDate.HasValue || productCreationDate <= EndDate;
        private bool IsUnitsOfMeasurementValid(string productUnitOfMeasurement) => UnitsOfMeasurement == null || !UnitsOfMeasurement.Any() || UnitsOfMeasurement.Contains(productUnitOfMeasurement);
        private bool IsMinRestValid(uint productCount) => !MinRest.HasValue || productCount >= MinRest;
        private bool IsMaxRestValid(uint productCount) => !MaxRest.HasValue || productCount <= MaxRest;
    }
}