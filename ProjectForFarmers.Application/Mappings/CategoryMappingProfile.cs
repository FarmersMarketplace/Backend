using AutoMapper;
using ProjectForFarmers.Application.ViewModels;
using ProjectForFarmers.Application.ViewModels.Category;
using ProjectForFarmers.Application.ViewModels.Subcategory;
using ProjectForFarmers.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.Mappings
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            MapCategoryToCategoryVm();
            MapSubcategoryToSubcategoryVm();
        }

        private void MapSubcategoryToSubcategoryVm()
        {
            CreateMap<Subcategory, SubcategoryVm>()
               .ForMember(vm => vm.Id, opt => opt.MapFrom(subcategory => subcategory.Id))
               .ForMember(vm => vm.Name, opt => opt.MapFrom(subcategory => subcategory.Name))
               .ForMember(vm => vm.CategoryId, opt => opt.MapFrom(subcategory => subcategory.CategoryId));
        }

        private void MapCategoryToCategoryVm()
        {
            CreateMap<Category, CategoryVm>()
               .ForMember(vm => vm.Id, opt => opt.MapFrom(category => category.Id))
               .ForMember(vm => vm.Name, opt => opt.MapFrom(category => category.Name))
               .ForMember(vm => vm.Subcategories, opt => opt.MapFrom(category => new List<SubcategoryVm>()));
        }
    }

}
