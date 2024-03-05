using FarmersMarketplace.Application.DataTransferObjects.Subcategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.Services.Business
{
    public interface ISubcategoryService
    {
        Task Create(CreateSubcategoryDto createSubcategoryDto);
        Task Update(UpdateSubcategoryDto updateSubcategoryDto);
        Task Delete(Guid subcategoryId);
    }
}
