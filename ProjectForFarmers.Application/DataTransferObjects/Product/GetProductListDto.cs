using ProjectForFarmers.Application.Filters;
using ProjectForFarmers.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.DataTransferObjects.Product
{
    public class GetProductListDto
    {
        public Guid ProducerId { get; set; }
        public Producer Producer { get; set; } 
        public ProductFilter? Filter { get; set; }
        public bool IncludeFilterData { get; set; }
        public DateTime Cursor { get; set; }
        public int PageSize { get; set; }
    }

}
