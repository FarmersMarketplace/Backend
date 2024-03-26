using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.ViewModels.Category
{
    public class CategoryListVm
    {
        public List<CategoryVm> Categories { get; set; }

        public CategoryListVm()
        {
            Categories = new List<CategoryVm>();
        }
    }

}
