using ProjectForFarmers.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.ViewModels.Product
{
    public class ProductVm
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ArticleNumber { get; set; }
        public Guid CategoryId { get; set; }
        public Guid SubcategoryId { get; set; }
        public string Category { get; set; }
        public string Subcategory { get; set; }
        public ProductStatus Status { get; set; }
        public Producer Producer { get; set; }
        public string ProducerName { get; set; }
        public string PackagingType { get; set; }
        public string UnitOfMeasurement { get; set; }
        public decimal PricePerOne { get; set; }
        public uint MinPurchaseQuantity { get; set; }
        public uint Count { get; set; }
        public List<ReceivingType> ReceivingTypes { get; set; }
        public List<string> ImagesNames { get; set; }
        public DateTime ExpirationDate { get; set; }
        public List<string> DocumentsNames { get; set; }
    }

}
