using FarmersMarketplace.Application.ViewModels.Category;
using FarmersMarketplace.Application.ViewModels.Subcategory;
using FarmersMarketplace.Domain;

namespace FarmersMarketplace.Application.ViewModels.Farm
{
    public class FarmVm
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ContactEmail { get; set; }
        public string OwnerName { get; set; }
        public AddressVm Address { get; set; }
        public ScheduleVm Schedule { get; set; }
        public PaymentDataVm PaymentData { get; set; }
        public List<ReceivingMethod>? ReceivingMethods { get; set; }
        public bool HasDelivery { get; set; }
        public List<string> ImagesNames { get; set; }
        public List<CategoryLookupVm>? Categories { get; set; }
        public List<SubcategoryVm>? Subcategories { get; set; }
        public List<FarmLogVm> Logs { get; set; }
        public string? FirstSocialPageUrl { get; internal set; }
        public string? SecondSocialPageUrl { get; internal set; }
    }
}
