using ProjectForFarmers.Application.ViewModels.Subcategory;
using ProjectForFarmers.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.ViewModels.Category
{
    public class CategoryVm
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<SubcategoryVm> Subcategories { get; set; }
    }

}
