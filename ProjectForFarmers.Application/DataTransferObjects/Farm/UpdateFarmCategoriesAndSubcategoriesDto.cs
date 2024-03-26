using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.DataTransferObjects.Farm
{
    public class UpdateFarmCategoriesAndSubcategoriesDto
    {
        public Guid FarmId { get; set; }
        public List<Guid> Categories { get; set; }
        public List<Guid> Subcategories { get; set; }
    }

}
