using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectForFarmers.Application.DataTransferObjects.Order;
using ProjectForFarmers.Application.DataTransferObjects.Product;
using ProjectForFarmers.Application.Exceptions;
using ProjectForFarmers.Application.Filters;
using ProjectForFarmers.Application.Helpers;
using ProjectForFarmers.Application.Interfaces;
using ProjectForFarmers.Application.ViewModels.Product;
using ProjectForFarmers.Domain;

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
            {
                string message = $"Product with Id {productId} was not found.";
                string userFacingMessage = CultureHelper.GetString("ProductWithIdNotFound", productId.ToString());

                throw new NotFoundException(message, userFacingMessage);
            }

            bool hasOrdersWithProduct = await DbContext.OrdersItems.AnyAsync(oi => oi.ProductId == productId);

            if (hasOrdersWithProduct)
            {
                string message = $"Product with Id {productId} is used in existing orders.";
                string userFacingMessage = CultureHelper.GetString("ProductIsUsed", productId.ToString());

                throw new NotFoundException(message, userFacingMessage);
            }
            else
            {
                await FileHelper.DeleteFiles(product.ImagesNames, Configuration["Images:Product"]);
                await FileHelper.DeleteFiles(product.DocumentsNames, Configuration["Documents"]);
                DbContext.Products.Remove(product);
                await DbContext.SaveChangesAsync();
            }
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
            {
                string message = $"Product with Id {productId} was not found.";
                string userFacingMessage = CultureHelper.GetString("ProductWithIdNotFound", productId.ToString());

                throw new NotFoundException(message, userFacingMessage);
            }

            var vm = Mapper.Map<ProductVm>(product);

            return vm;
        }

        public async Task<ProductsListVm> GetAll(GetProductListDto getProductListDto)
        {
            var productsQuery = DbContext.Products.Where(p => p.CreationDate < getProductListDto.Cursor 
                && p.ProducerId == getProductListDto.ProducerId
                && p.Producer == getProductListDto.Producer);

            if(getProductListDto.Filter != null)
            {
                productsQuery = await getProductListDto.Filter.ApplyFilter(productsQuery);
            }

            var products = await productsQuery.OrderByDescending(product => product.CreationDate)
                .Take(getProductListDto.PageSize)
                .ToListAsync();

            var vm = new ProductsListVm
            {
                Products = new List<ProductLookupVm>()
            };

            foreach(var product in products)
            {
                var productVm = Mapper.Map<ProductLookupVm>(product);
                vm.Products.Add(productVm);
            }

            vm.Count = await productsQuery.CountAsync();

            if (getProductListDto.IncludeFilterData == true)
            {
                var unitsOfMeasurements = new HashSet<string>();

                foreach (var product in products)
                {
                    vm.Products.Add(Mapper.Map<ProductLookupVm>(product));
                    if (!unitsOfMeasurements.Contains(product.UnitOfMeasurement))
                        unitsOfMeasurements.Add(product.UnitOfMeasurement);
                }

                vm.FilterData = new FilterData { UnitsOfMeasurement = unitsOfMeasurements.ToList() };
            }
            else
            {
                vm.FilterData = null;
            }

            return vm;
        }

        public async Task Update(UpdateProductDto updateProductDto)
        {
            var product = await DbContext.Products.FirstAsync(p => p.Id == updateProductDto.Id);

            if (product == null)
            {
                string message = $"Product with Id {updateProductDto.Id} was not found.";
                string userFacingMessage = CultureHelper.GetString("ProductWithIdNotFound", updateProductDto.Id.ToString());

                throw new NotFoundException(message, userFacingMessage);
            }

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
            product.ExpirationDate = updateProductDto.ExpirationDate;
            product.CreationDate = updateProductDto.CreationDate;

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
