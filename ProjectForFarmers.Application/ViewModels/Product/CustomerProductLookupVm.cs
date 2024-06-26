﻿using FarmersMarketplace.Domain;

namespace FarmersMarketplace.Application.ViewModels.Product
{
    public class CustomerProductLookupVm
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
        public decimal PricePerOne { get; set; }
        public ProductStatus Status { get; set; }
    }

}
