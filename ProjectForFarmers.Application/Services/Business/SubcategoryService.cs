using AutoMapper;
using Microsoft.Extensions.Configuration;
using ProjectForFarmers.Application.DataTransferObjects.Catefory;
using ProjectForFarmers.Application.DataTransferObjects.Subcategory;
using ProjectForFarmers.Application.Exceptions;
using ProjectForFarmers.Application.Interfaces;
using ProjectForFarmers.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.Services.Business
{
    public class SubcategoryService : Service, ISubcategoryService
    {
        public SubcategoryService(IMapper mapper, IApplicationDbContext dbContext, IConfiguration configuration) : base(mapper, dbContext, configuration)
        {
        }

        public async Task Create(CreateSubcategoryDto createSubcategoryDto)
        {
            var category = DbContext.Categories.FirstOrDefault(c => c.Id == createSubcategoryDto.CategoryId);

            if (category != null)
                throw new NotFoundException($"Category with Id {createSubcategoryDto.CategoryId} was not found.");

            Guid id = Guid.NewGuid();
            var subcategory = new Subcategory
            {
                Id = id,
                Name = createSubcategoryDto.Name,
                CategoryId = createSubcategoryDto.CategoryId
            };

            DbContext.Subcategories.Add(subcategory);
            await DbContext.SaveChangesAsync();
        }

        public async Task Delete(Guid subcategoryId)
        {
            var subcategory = DbContext.Categories.FirstOrDefault(c => c.Id == subcategoryId);

            if (subcategory != null)
                throw new NotFoundException($"Subcategory with Id {subcategoryId} was not found.");

            DbContext.Categories.Remove(subcategory);
            await DbContext.SaveChangesAsync();
        }

        public async Task Update(UpdateSubcategoryDto updateSubcategoryDto)
        {
            var subcategory = DbContext.Categories.FirstOrDefault(c => c.Id == updateSubcategoryDto.Id);

            if (subcategory != null)
                throw new NotFoundException($"Subcategory with Id {updateSubcategoryDto.Id} was not found.");

            subcategory.Name = updateSubcategoryDto.Name;

            await DbContext.SaveChangesAsync();
        }
    }

}
