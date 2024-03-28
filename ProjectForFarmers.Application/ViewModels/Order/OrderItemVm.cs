using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.ViewModels.Order
{
    public class OrderItemVm
    {
        public Guid Id { get; set; }
        public string? PhotoName { get; set; }
        public string Name { get; set; }
        public string ArticleNumber { get; set; }
        public uint Count { get; set; }
        public decimal PricePerOne { get; set; }
        public decimal TotalPrice { get; set; }
        public string UnitOfMeasurement { get; set; }
    }

}
