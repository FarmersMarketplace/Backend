using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.DataTransferObjects.Subcategory
{
    public class CreateSubcategoryDto
    {
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
    }

}
