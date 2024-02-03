using ProjectForFarmers.Application.DataTransferObjects.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.Services.Business
{
    public interface IProductService
    {
        Task Create(CreateProductDto createProductDto);
    }

}
