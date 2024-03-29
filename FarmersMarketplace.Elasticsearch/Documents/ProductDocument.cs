using FarmersMarketplace.Domain.Feedbacks;
using FarmersMarketplace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Elasticsearch.Documents
{
    public class ProductDocument
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ArticleNumber { get; set; }
        public Guid SubcategoryId { get; set; }
        public string SubcategoryName { get; set; }
        public string CategoryName { get; set; }
        public ProductStatus Status { get; set; }
        public Producer Producer { get; set; }
        public Guid ProducerId { get; set; }
        public string PackagingType { get; set; }
        public string UnitOfMeasurement { get; set; }
        public decimal PricePerOne { get; set; }
        public uint Count { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime CreationDate { get; set; }
        public string ImageName { get; set; }
        public float Rating { get; set; }
        public uint FeedbacksCount { get; set; }
    }

}
