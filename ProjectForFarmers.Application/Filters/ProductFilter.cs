﻿using FarmersMarketplace.Application.ViewModels.Product;
using FarmersMarketplace.Domain;

namespace FarmersMarketplace.Application.Filters
{
    public class ProductFilter : IFilter<Product>
    {
        public List<Guid>? CategoryIds { get; set; }
        public List<Guid>? SubcategoryIds { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<string>? UnitsOfMeasurement { get; set; }
        public uint? MinRest { get; set; }
        public uint? MaxRest { get; set; }

        public async Task<IQueryable<Product>> ApplyFilter(IQueryable<Product> query)
        {
            return query
                    .Where(p =>
                        (CategoryIds == null || !CategoryIds.Any() || CategoryIds.Contains(p.CategoryId)) &&
                        (SubcategoryIds == null || !SubcategoryIds.Any() || SubcategoryIds.Contains(p.SubcategoryId)) &&
                        (!StartDate.HasValue || p.CreationDate >= StartDate) &&
                        (!EndDate.HasValue || p.CreationDate <= EndDate) &&
                        (UnitsOfMeasurement == null || !UnitsOfMeasurement.Any() || UnitsOfMeasurement.Contains(p.UnitOfMeasurement)) &&
                        (!MinRest.HasValue || p.Count >= MinRest) &&
                        (!MaxRest.HasValue || p.Count <= MaxRest));
        }
    }

}