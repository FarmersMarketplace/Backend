using AutoMapper;
using FarmersMarketplace.Application.ViewModels;
using FarmersMarketplace.Application.ViewModels.Category;
using FarmersMarketplace.Application.ViewModels.Subcategory;
using FarmersMarketplace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.Mappings
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            MapCategoryToCategoryVm();
            MapSubcategoryToSubcategoryVm();
        }

        private void MapSubcategoryToSubcategoryVm()
        {
            CreateMap<Subcategory, SubcategoryVm>();
        }

        private void MapCategoryToCategoryVm()
        {
            CreateMap<Category, CategoryVm>();
        }
    }

}
