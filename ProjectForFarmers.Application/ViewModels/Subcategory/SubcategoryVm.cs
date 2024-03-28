namespace FarmersMarketplace.Application.ViewModels.Subcategory
{
    public class SubcategoryVm
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageName { get; set; }
        public Guid CategoryId { get; set; }

        public SubcategoryVm(Guid id, string name, Guid categoryId)
        {
            Id = id;
            Name = name;
            CategoryId = categoryId;
        }
    }

}
