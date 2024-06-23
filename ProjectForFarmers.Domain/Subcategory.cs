namespace FarmersMarketplace.Domain
{
    public class Subcategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageName { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
