using Microsoft.AspNetCore.Http;
using FarmersMarketplace.Domain;

namespace FarmersMarketplace.Application.DataTransferObjects.Product
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public Guid SubcategoryId { get; set; }
        public Producer Producer { get; set; }
        public Guid ProducerId { get; set; }
        public string PackagingType { get; set; }
        public string UnitOfMeasurement { get; set; }
        public decimal PricePerOne { get; set; }
        public int MinPurchaseQuantity { get; set; }
        public int Count { get; set; }
        public List<IFormFile>? Images { get; set; }
        public DateTime ExpirationDate { get; set; }
        public List<IFormFile>? Documents { get; set; }
        public ProductStatus Status { get; set; }
    }

}
