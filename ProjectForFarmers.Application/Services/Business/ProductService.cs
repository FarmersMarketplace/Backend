using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectForFarmers.Application.DataTransferObjects.Product;
using ProjectForFarmers.Application.Exceptions;
using ProjectForFarmers.Application.Filters;
using ProjectForFarmers.Application.Helpers;
using ProjectForFarmers.Application.Interfaces;
using ProjectForFarmers.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.Services.Business
{
    public class ProductService : Service, IProductService
    {
        private readonly FileHelper FileHelper;

        public ProductService(IMapper mapper, IApplicationDbContext dbContext, IConfiguration configuration) : base(mapper, dbContext, configuration)
        {
            FileHelper = new FileHelper();
        }

        public async Task Create(ProductDto productDto)
        {
            var product = Mapper.Map<Product>(productDto);
            product.ArticleNumber = GenerateArticleNumber();

            product.ImagesNames = await FileHelper.SaveImages(productDto.Images, Configuration["Images:Product"]);
            product.DocumentsNames = await FileHelper.SaveFiles(productDto.Documents, Configuration["Documents"]);

            DbContext.Products.Add(product);
            await DbContext.SaveChangesAsync();
        }

        public async Task Delete(Guid productId)
        {
            var product = await DbContext.Products.FirstAsync(p => p.Id == productId);

            if (product == null)
                throw new NotFoundException($"Product with Id {productId} was not found.");

            DbContext.Products.Remove(product);
            await DbContext.SaveChangesAsync();
        }

        public string GenerateArticleNumber()
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string numbers = "0123456789";

            Random random = new Random();

            string randomLetters = new string(Enumerable.Repeat(letters, 4)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            string randomNumbers = new string(Enumerable.Repeat(numbers, 4)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return randomLetters + "-" + randomNumbers;
        }

        public Task Get(Guid productId)
        {
            throw new NotImplementedException();
        }

        public Task GetAll(Guid producerId, Producer producer, ProductFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task Update(Guid productId, ProductDto productDto)
        {
            throw new NotImplementedException();
        }
    }

}
