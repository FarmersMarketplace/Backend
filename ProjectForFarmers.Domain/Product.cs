using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Domain
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public Guid SubcategoryId { get; set; }
        public Category Category { get; set; }
        public Subcategory Subcategory { get; set; }
        public ProductStatus Status { get; set; }
        public Producer Producer { get; set; }
        public Guid ProducerId { get; set; }
        public string PackagingType { get; set; }
        public string UnitOfMeasurement { get; set; }
        public string PricePerOne { get; set; }
        public int MinPurchaseQuantity { get; set; }
        public int Count { get; set; }
        public List<PaymentType> PaymentTypes { get; set; }
        public List<ReceivingType> ReceivingTypes { get; set; }
        public Guid SupplyPointId { get; set; }
        public Address SupplyPoint { get; set; }
        public List<string> ImagesNames { get; set; }
        public DateTime ExpirationDate { get; set; }
        public List<string> DocumentsNames { get; set; }
    }

}