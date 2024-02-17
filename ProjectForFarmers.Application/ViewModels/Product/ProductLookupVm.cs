using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.ViewModels.Product
{
    public class ProductLookupVm
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ArticleNumber { get; set; }
        public string Category { get; set; }
        public string Subcategory { get; set; }
        public uint Rest {  get; set; }
        public string UnitOfMeasurement {  get; set; }
        public decimal PricePerOne { get; set; }
        public DateTime CreationDate { get; set; }
    }

}
