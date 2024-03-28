using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Domain
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } 
        public string Description { get; set; }
        public string ArticleNumber { get; set; }
        public Guid CategoryId { get; set; }
        public Guid SubcategoryId { get; set; }
        public Category Category { get; set; }
        public Subcategory Subcategory { get; set; }
        public ProductStatus Status { get; set; }
        public Producer Producer { get; set; }
        public Guid ProducerId { get; set; }
        public string PackagingType { get; set; }
        public string UnitOfMeasurement { get; set; }
        public decimal PricePerOne { get; set; }
        public uint MinPurchaseQuantity { get; set; }
        public uint Count { get; set; }
        public List<string> ImagesNames { get; set; }
        public uint ExpirationDate { get; set; }
        public DateTime CreationDate { get; set; }
        public List<string> DocumentsNames { get; set; }
        public List<ReceivingMethod> ReceivingMethods { get; set; }
    }

}