using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.DataTransferObjects.Account
{
    public class SellerCategoriesAndSubcategoriesDto
    {
        public List<Guid>? Categories { get; set; }
        public List<Guid>? Subcategories { get; set; }
    }

}
