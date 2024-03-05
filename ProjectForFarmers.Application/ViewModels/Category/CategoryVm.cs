using FarmersMarketplace.Application.ViewModels.Subcategory;
using FarmersMarketplace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.ViewModels.Category
{
    public class CategoryVm
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<SubcategoryVm> Subcategories { get; set; }
    }

}
