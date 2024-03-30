using FarmersMarketplace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.ViewModels.Product
{
    public class CustomerProductVm
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Producer Producer { get; set; }
        public string ProducerName { get; set; }
        public string ProducerImageName { get; set; }
        public string ImageName { get; set; }
        public DateTime ExpirationDate { get; set; }
        public uint FeedbacksCount { get; set; }
        public float Rating { get; set; }
        public decimal Price { get; set; }
    }

}
