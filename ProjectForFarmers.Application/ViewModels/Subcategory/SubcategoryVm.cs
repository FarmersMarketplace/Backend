using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.ViewModels.Subcategory
{
    public class SubcategoryVm
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CategoryId { get; set; }

        public SubcategoryVm(Guid id, string name, Guid categoryId)
        {
            Id = id;
            Name = name;
            CategoryId = categoryId;
        }
    }

}
