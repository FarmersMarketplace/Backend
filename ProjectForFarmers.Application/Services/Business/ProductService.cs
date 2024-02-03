using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectForFarmers.Application.DataTransferObjects.Product;
using ProjectForFarmers.Application.Exceptions;
using ProjectForFarmers.Application.Filters;
using ProjectForFarmers.Application.Helpers;
using ProjectForFarmers.Application.Interfaces;
using ProjectForFarmers.Application.ViewModels.Product;
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

            product.ImagesNames = await FileHelper.SaveImages(createProductDto.Images, Configuration["Images:Product"]);
            product.DocumentsNames = await FileHelper.SaveFiles(createProductDto.Documents, Configuration["Documents"]);

            DbContext.Products.Add(product);
            await DbContext.SaveChangesAsync();
        }

        public async Task Delete(Guid productId)
        {
            var product = await DbContext.Products.FirstAsync(p => p.Id == productId);

            if (product == null)
                throw new NotFoundException($"Product with Id {productId} was not found.");

            await FileHelper.DeleteFiles(product.ImagesNames, Configuration["Images:Product"]); 

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

        public async Task<ProductVm> Get(Guid productId)
        {
            var product = await DbContext.Products.Include(p => p.Category)
                .Include(p => p.Subcategory)
                .FirstAsync(p => p.Id == productId);

            if (product == null)
                throw new NotFoundException($"Product with Id {productId} was not found.");

            var vm = Mapper.Map<ProductVm>(product);

            return vm;
        }

        public async Task GetAll(Guid producerId, Producer producer, ProductFilter filter)
        {
            
        }

        public async Task Update(UpdateProductDto updateProductDto)
        {
            var product = await DbContext.Products.FirstAsync(p => p.Id == updateProductDto.Id);

            if (product == null)
                throw new NotFoundException($"Product with Id {updateProductDto.Id} was not found.");

            product.Name = updateProductDto.Name;
            product.Description = updateProductDto.Description;
            product.CategoryId = updateProductDto.CategoryId;
            product.SubcategoryId = updateProductDto.SubcategoryId;
            product.PackagingType = updateProductDto.PackagingType;
            product.Status = updateProductDto.Status;
            product.UnitOfMeasurement = updateProductDto.UnitOfMeasurement;
            product.PricePerOne = updateProductDto.PricePerOne;
            product.MinPurchaseQuantity = updateProductDto.MinPurchaseQuantity;
            product.Count = updateProductDto.Count;
            product.ReceivingTypes = updateProductDto.ReceivingTypes;
            product.ExpirationDate = updateProductDto.ExpirationDate;

            if (updateProductDto.Images != null && product.ImagesNames != null)
            {
                foreach (var imageName in product.ImagesNames)
                {
                    if (!updateProductDto.Images.Any(file => file.FileName == imageName))
                    {
                        FileHelper.DeleteFile(imageName, Configuration["Images:Product"]);
                    }
                }
            }

            if (updateProductDto.Documents != null && product.DocumentsNames != null)
            {
                foreach (var documentName in product.DocumentsNames)
                {
                    if (!updateProductDto.Documents.Any(file => file.FileName == documentName))
                    {
                        FileHelper.DeleteFile(documentName, Configuration["Documents"]);
                    }
                }
            }

            if (updateProductDto.Images != null)
            {
                foreach (var newImage in updateProductDto.Images)
                {
                    if (!product.ImagesNames.Contains(newImage.FileName))
                    {
                        string imageName = await FileHelper.SaveFile(newImage, Configuration["Images:Product"]);
                        product.ImagesNames.Add(imageName);
                    }
                }
            }

            if (updateProductDto.Documents != null)
            {
                foreach (var newDocument in updateProductDto.Documents)
                {
                    if (!product.DocumentsNames.Contains(newDocument.FileName))
                    {
                        string documentName = await FileHelper.SaveFile(newDocument, Configuration["Documents"]);
                        product.DocumentsNames.Add(documentName);
                    }
                }
            }

            await DbContext.SaveChangesAsync();
        }
    }

}
