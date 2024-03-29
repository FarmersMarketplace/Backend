using Microsoft.AspNetCore.Http;
using FarmersMarketplace.Domain;

namespace FarmersMarketplace.Application.DataTransferObjects.Product
{
    public class UpdateProductDto
    {
        public Guid Id { get; set; }
        public Guid ProducerId { get; set; }
        public Producer Producer {  get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public Guid SubcategoryId { get; set; }
        public ProductStatus Status { get; set; }
        public string PackagingType { get; set; }
        public string UnitOfMeasurement { get; set; }
        public decimal PricePerOne { get; set; }
        public uint MinPurchaseQuantity { get; set; }
        public uint Count { get; set; }
        public List<ReceivingMethod> ReceivingTypes { get; set; }
        public List<IFormFile>? Images { get; set; }
        public uint ExpirationDate { get; set; }
        public DateTime CreationDate { get; set; }
        public List<IFormFile>? Documents { get; set; }
    }

}
