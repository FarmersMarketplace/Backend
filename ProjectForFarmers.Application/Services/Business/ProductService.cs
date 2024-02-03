using AutoMapper;
using Microsoft.Extensions.Configuration;
using ProjectForFarmers.Application.DataTransferObjects.Product;
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

        public async Task Create(CreateProductDto createProductDto)
        {
            var product = Mapper.Map<Product>(createProductDto);
            product.ArticleNumber = GenerateArticleNumber();

            product.ImagesNames = await FileHelper.SaveImages(createProductDto.Images, Configuration["Images:Farms"]);
            product.DocumentsNames = await FileHelper.SaveFiles(createProductDto.Documents, Configuration["Documents"]);

            DbContext.
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
    }

}
