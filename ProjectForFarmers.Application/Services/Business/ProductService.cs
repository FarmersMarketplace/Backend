using AutoMapper;
using FastExcel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using FarmersMarketplace.Application.DataTransferObjects.Product;
using FarmersMarketplace.Application.Exceptions;
using FarmersMarketplace.Application.Filters;
using FarmersMarketplace.Application.Helpers;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Application.ViewModels.Dashboard;
using FarmersMarketplace.Application.ViewModels.Product;
using FarmersMarketplace.Domain;
using System.Data;
using InvalidDataException = FarmersMarketplace.Application.Exceptions.InvalidDataException;

namespace FarmersMarketplace.Application.Services.Business
{
    public class ProductService : Service, IProductService
    {
        private readonly FileHelper FileHelper;
        private readonly IMemoryCache MemoryCache;
        private readonly string ProductsImagesFolder;
        private readonly string DocumentsFolder;

        public ProductService(IMapper mapper, IApplicationDbContext dbContext, IConfiguration configuration, IMemoryCache memoryCache) : base(mapper, dbContext, configuration)
        {
            FileHelper = new FileHelper();
            MemoryCache = memoryCache;
            ProductsImagesFolder = Configuration["File:Images:Product"];
            DocumentsFolder = Configuration["File:Documents"];
        }

        public async Task<OptionListVm> Autocomplete(ProductAutocompleteDto productAutocompleteDto)
        {
            var cacheKey = CacheHelper.GenerateCacheKey<ProductFilter>(productAutocompleteDto.ProducerId, productAutocompleteDto.Producer, "products");

            var vm = new OptionListVm();
            var productsInfo = new List<ProductInfo>();

            if (!MemoryCache.TryGetValue(cacheKey, out productsInfo))
            {
                productsInfo = await DbContext.Products
                    .Where(p => p.ProducerId == productAutocompleteDto.ProducerId && p.Producer == productAutocompleteDto.Producer)
                    .Select(p => new ProductInfo { Name = p.Name, ArticleNumber = p.ArticleNumber })
                    .Distinct()
                    .ToListAsync();

                MemoryCache.Set(cacheKey, productsInfo, TimeSpan.FromMinutes(10));
            }

            int added = 0;

            if(!productAutocompleteDto.Query.IsNullOrEmpty()) productAutocompleteDto.Query = productAutocompleteDto.Query.Trim();

            for (int i = 0; i < productsInfo.Count && added < productAutocompleteDto.Count; i++)
            {
                if (productsInfo[i].Name.Contains(productAutocompleteDto.Query, StringComparison.OrdinalIgnoreCase)
                     && !vm.Options.Contains(productsInfo[i].Name))
                {
                    vm.Options.Add(productsInfo[i].Name);
                    added++;
                }

                if (productsInfo[i].ArticleNumber.Contains(productAutocompleteDto.Query, StringComparison.OrdinalIgnoreCase)
                    && !vm.Options.Contains(productsInfo[i].ArticleNumber))
                {
                    vm.Options.Add(productsInfo[i].ArticleNumber);
                    added++;
                }
            }

            return vm;
        }

        public async Task Create(CreateProductDto createProductDto)
        {
            var product = Mapper.Map<Product>(createProductDto);

            var category = await DbContext.Categories.FirstOrDefaultAsync(c => c.Id == product.CategoryId);
            if (category == null)
            {
                string message = $"Category with Id {product.CategoryId} was not found.";
                string userFacingMessage = CultureHelper.Exception("CategoryWithIdNotFound", product.CategoryId.ToString());

                throw new NotFoundException(message, userFacingMessage);
            }

            var subcategory = await DbContext.Subcategories.FirstOrDefaultAsync(c => c.Id == product.SubcategoryId);
            if (subcategory == null)
            {
                string message = $"Subcategory with Id {product.SubcategoryId} was not found.";
                string userFacingMessage = CultureHelper.Exception("SubcategoryWithIdNotFound", product.SubcategoryId.ToString());

                throw new NotFoundException(message, userFacingMessage);
            }
            else if(subcategory.CategoryId != category.Id)
            {
                string message = $"Subcategory does not belong to the category.";
                string userFacingMessage = CultureHelper.Exception("SubcategoryNotBelongsToCategory", product.SubcategoryId.ToString(), product.CategoryId.ToString());

                throw new InvalidDataException(message, userFacingMessage);
            }

            product.ArticleNumber = GenerateArticleNumber();
            product.ImagesNames = await FileHelper.SaveImages(createProductDto.Images, ProductsImagesFolder);
            product.DocumentsNames = await FileHelper.SaveFiles(createProductDto.Documents, DocumentsFolder);

            DbContext.Products.Add(product);
            await DbContext.SaveChangesAsync();
        }

        public void Validate(Guid accountId, Guid producerId, Producer producer)
        {
            if (producer == Producer.Seller)
            {
                if (producerId != accountId)
                {
                    string message = $"Access denied: Permission denied to modify data.";
                    string userFacingMessage = CultureHelper.Exception("AccessDenied");

                    throw new AuthorizationException(message, userFacingMessage);
                }
            }
            else if (producer == Producer.Farm)
            {
                var farm = DbContext.Farms.FirstOrDefault(f => f.Id == producerId);
                if (farm == null)
                {
                    string message = $"Farm with Id {producerId} was not found.";
                    string userFacingMessage = CultureHelper.Exception("FarmWithIdNotFound", producerId.ToString());

                    throw new NotFoundException(message, userFacingMessage);
                }
                if (farm.OwnerId != accountId)
                {
                    string message = $"Access denied: Permission denied to modify data.";
                    string userFacingMessage = CultureHelper.Exception("AccessDenied");

                    throw new AuthorizationException(message, userFacingMessage);
                }
            }
            else
            {
                throw new Exception("Producer is not validated.");
            }
        }

        public async Task Delete(ProductListDto productListDto, Guid accountId)
        {
            foreach (var productId in productListDto.Products)
            {
                var product = await DbContext.Products.FirstAsync(p => p.Id == productId);

                if (product == null)
                {
                    string message = $"Product with Id {productId} was not found.";
                    string userFacingMessage = CultureHelper.Exception("ProductWithIdNotFound", productId.ToString());

                    throw new NotFoundException(message, userFacingMessage);
                }
                Validate(accountId, product.ProducerId, product.Producer);

                bool hasOrdersWithProduct = await DbContext.OrdersItems.AnyAsync(oi => oi.ProductId == productId);

                if (hasOrdersWithProduct)
                {
                    string message = $"Product with Id {productId} is used in existing orders.";
                    string userFacingMessage = CultureHelper.Exception("ProductIsUsed", productId.ToString());

                    throw new NotFoundException(message, userFacingMessage);
                }
                else
                {
                    await FileHelper.DeleteFiles(product.ImagesNames, ProductsImagesFolder);
                    await FileHelper.DeleteFiles(product.DocumentsNames, DocumentsFolder);
                    DbContext.Products.Remove(product);
                    await DbContext.SaveChangesAsync();
                }
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
                string userFacingMessage = CultureHelper.Exception("ProductWithIdNotFound", productId.ToString());

                throw new NotFoundException(message, userFacingMessage);
            }

            var vm = Mapper.Map<ProductVm>(product);

            return vm;
        }

        public async Task<ProductListVm> GetAll(GetProductListDto getProductListDto)
        {
            var productsQuery = DbContext.Products.Include(p => p.Category)
                .Include(p => p.Subcategory)
                .Where(p => p.CreationDate < getProductListDto.Cursor 
                && p.ProducerId == getProductListDto.ProducerId
                && p.Producer == getProductListDto.Producer);

            if (!getProductListDto.Query.IsNullOrEmpty())
            {
                getProductListDto.Query = getProductListDto.Query.Trim().ToLower();
                productsQuery = productsQuery.Where(p =>
                    p.Name.ToLower().Contains(getProductListDto.Query) ||
                    p.ArticleNumber.ToLower().Contains(getProductListDto.Query));

            }

            if (getProductListDto.Filter != null)
            {
                productsQuery = await getProductListDto.Filter.ApplyFilter(productsQuery);
            }

            var products = await productsQuery.OrderByDescending(product => product.CreationDate)
                .ToListAsync();

            var vm = new ProductListVm
            {
                Products = new List<ProductLookupVm>(),
                Count = products.Count
            };

            products = products.Take(getProductListDto.PageSize).ToList();

            foreach (var product in products)
            {
                var productVm = Mapper.Map<ProductLookupVm>(product);
                vm.Products.Add(productVm);
            }

            return vm;
        }

        public async Task Update(UpdateProductDto updateProductDto, Guid accountId)
        {
            var category = await DbContext.Categories.FirstOrDefaultAsync(c => c.Id == updateProductDto.CategoryId);
            if (category == null)
            {
                string message = $"Category with Id {updateProductDto.CategoryId} was not found.";
                string userFacingMessage = CultureHelper.Exception("CategoryWithIdNotFound", updateProductDto.CategoryId.ToString());

                throw new NotFoundException(message, userFacingMessage);
            }

            var subcategory = await DbContext.Subcategories.FirstOrDefaultAsync(c => c.Id == updateProductDto.SubcategoryId);
            if (subcategory == null)
            {
                string message = $"Subcategory with Id {updateProductDto.SubcategoryId} was not found.";
                string userFacingMessage = CultureHelper.Exception("SubcategoryWithIdNotFound", updateProductDto.SubcategoryId.ToString());

                throw new NotFoundException(message, userFacingMessage);
            }
            else if (subcategory.CategoryId != category.Id)
            {
                string message = $"Subcategory does not belong to the category.";
                string userFacingMessage = CultureHelper.Exception("SubcategoryNotBelongsToCategory", updateProductDto.SubcategoryId.ToString(), updateProductDto.CategoryId.ToString());

                throw new InvalidDataException(message, userFacingMessage);
            }

            var product = await DbContext.Products.FirstAsync(p => p.Id == updateProductDto.Id);

            if (product == null)
            {
                string message = $"Product with Id {updateProductDto.Id} was not found.";
                string userFacingMessage = CultureHelper.Exception("ProductWithIdNotFound", updateProductDto.Id.ToString());

                throw new NotFoundException(message, userFacingMessage);
            }
            Validate(accountId, product.ProducerId, product.Producer);

            if (product.Producer != updateProductDto.Producer 
                || product.ProducerId != updateProductDto.ProducerId)
            {
                string message = $"Access denied: Permission denied to modify data.";
                string userFacingMessage = CultureHelper.Exception("AccessDenied");

                throw new AuthorizationException(message, userFacingMessage);
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

            if (updateProductDto.Images == null) updateProductDto.Images = new List<IFormFile>();
            if (updateProductDto.Documents == null) updateProductDto.Documents = new List<IFormFile>();

            if (product.ImagesNames == null) product.ImagesNames = new List<string>();
            if (product.DocumentsNames == null) product.DocumentsNames = new List<string>();

            foreach (var imageName in product.ImagesNames.ToList())
            {
                if (!updateProductDto.Images.Any(file => file.FileName == imageName))
                {
                    FileHelper.DeleteFile(imageName, ProductsImagesFolder);
                    product.ImagesNames.Remove(imageName);
                }
            }

            foreach (var documentName in product.DocumentsNames.ToList())
            {
                if (!updateProductDto.Documents.Any(file => file.FileName == documentName))
                {
                    FileHelper.DeleteFile(documentName, DocumentsFolder);
                    product.DocumentsNames.Remove(documentName);
                }
            }

            foreach (var newImage in updateProductDto.Images)
            {
                if (!product.ImagesNames.Contains(newImage.FileName))
                {
                    string imageName = await FileHelper.SaveFile(newImage, ProductsImagesFolder);
                    product.ImagesNames.Add(imageName);
                }
            }

            foreach (var newDocument in updateProductDto.Documents)
            {
                if (!product.DocumentsNames.Contains(newDocument.FileName))
                {
                    string documentName = await FileHelper.SaveFile(newDocument, DocumentsFolder);
                    product.DocumentsNames.Add(documentName);
                }
            }

            await DbContext.SaveChangesAsync();
        }

        public async Task Duplicate(ProductListDto productListDto, Guid accountId)
        {
            foreach(var productId in productListDto.Products)
            {
                var product = await DbContext.Products.FirstOrDefaultAsync(p => p.Id == productId);

                if (product == null)
                {
                    string message = $"Product with Id {productId} was not found.";
                    string userFacingMessage = CultureHelper.Exception("ProductWithIdNotFound", productId.ToString());

                    throw new NotFoundException(message, userFacingMessage);
                }
                Validate(accountId, product.ProducerId, product.Producer);

                var newProduct = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = product.Name,
                    Description = product.Description,
                    ArticleNumber = GenerateArticleNumber(),
                    CategoryId = product.CategoryId,
                    SubcategoryId = product.SubcategoryId,
                    Status = product.Status,
                    ProducerId = product.ProducerId,
                    Producer = product.Producer,
                    PackagingType = product.PackagingType,
                    UnitOfMeasurement = product.UnitOfMeasurement,
                    PricePerOne = product.PricePerOne,
                    MinPurchaseQuantity = product.MinPurchaseQuantity,
                    Count = product.Count,
                    ExpirationDate = product.ExpirationDate,
                    CreationDate = product.CreationDate,
                    ImagesNames = new List<string>(),
                    DocumentsNames = new List<string>()
                };

                foreach(var imageName in product.ImagesNames)
                {
                    var newImageName = await FileHelper.CopyFile(Path.Combine(ProductsImagesFolder, imageName), ProductsImagesFolder);
                    newProduct.ImagesNames.Add(newImageName);
                }

                foreach (var documentsName in product.DocumentsNames)
                {
                    var newDocumentName = await FileHelper.CopyFile(Path.Combine(DocumentsFolder, documentsName), DocumentsFolder);
                    newProduct.DocumentsNames.Add(newDocumentName);
                }

                DbContext.Products.Add(newProduct);
            }

            await DbContext.SaveChangesAsync();
        }

        public async Task<FilterData> GetFilterData(Producer producer, Guid producerId)
        {
            var unitsOfMeasurement = await DbContext.Products.Where(p => p.Producer == producer
                && p.ProducerId == producerId)
                .Select(p => p.UnitOfMeasurement)
                .ToListAsync();

            var vm = new FilterData();
            vm.UnitsOfMeasurement = new HashSet<string>(unitsOfMeasurement);

            return vm;
        }

        public async Task<(string fileName, byte[] bytes)> ExportToExcel(ExportProductsDto exportProductsDto)
        {
            var productsQuery = DbContext.Products.Include(p => p.Category)
                .Include(p => p.Subcategory)
                .Where(p => p.ProducerId == exportProductsDto.ProducerId 
                && p.Producer == exportProductsDto.Producer);

            if(exportProductsDto.Filter != null)
            {
                productsQuery = await exportProductsDto.Filter.ApplyFilter(productsQuery);
            }

            List<ProductLookupVm> products = await productsQuery.Select(p => Mapper.Map<ProductLookupVm>(p)).ToListAsync();

            string fileName = await GetFileName(exportProductsDto.ProducerId, exportProductsDto.Producer);
            string filePath = Path.Combine(Configuration["File:Temporary"], fileName);
            string templatePath = Path.Combine(Configuration["File:Temporary"], "template.xlsx");

            using (var fastExcel = new FastExcel.FastExcel(new FileInfo(templatePath), new FileInfo(filePath)))
            {
                var worksheet = new Worksheet();
                var rows = new List<Row>();
                var cells = new List<Cell>();

                cells.Add(new Cell(1, CultureHelper.Property("Id")));
                cells.Add(new Cell(2, CultureHelper.Property("Name")));
                cells.Add(new Cell(3, CultureHelper.Property("ArticleNumber")));
                cells.Add(new Cell(4, CultureHelper.Property("Category")));
                cells.Add(new Cell(5, CultureHelper.Property("Subcategory")));
                cells.Add(new Cell(6, CultureHelper.Property("Rest")));
                cells.Add(new Cell(7, CultureHelper.Property("UnitOfMeasurement")));
                cells.Add(new Cell(8, CultureHelper.Property("PricePerOne")));
                cells.Add(new Cell(9, CultureHelper.Property("CreationDate")));

                rows.Add(new Row(1, cells));

                for (int i = 0; i < products.Count; i++)
                {
                    cells = new List<Cell>();

                    cells.Add(new Cell(1, products[i].Id.ToString()));
                    cells.Add(new Cell(2, products[i].Name));
                    cells.Add(new Cell(3, products[i].ArticleNumber));
                    cells.Add(new Cell(4, products[i].Category));
                    cells.Add(new Cell(5, products[i].Subcategory));
                    cells.Add(new Cell(6, products[i].Rest.ToString()));
                    cells.Add(new Cell(7, products[i].UnitOfMeasurement));
                    cells.Add(new Cell(8, products[i].PricePerOne.ToString()));
                    cells.Add(new Cell(9, products[i].CreationDate.ToString("dd.MM.yyyy HH:mm:ss")));

                    rows.Add(new Row(i + 2, cells));
                }

                worksheet.Rows = rows;
                fastExcel.Write(worksheet, "sheet1");
            }

            byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            System.IO.File.Delete(filePath);

            return (fileName, fileBytes);
        }

        private async Task<string> GetFileName(Guid producerId, Producer producer)
        {
            string producerName = "";

            if (producer == Producer.Seller)
            {
                var account = await DbContext.Accounts.FirstOrDefaultAsync(a => a.Id == producerId
                    && a.Role == Role.Seller);

                if (account == null)
                {
                    string message = $"Account with Id {producerId} was not found.";
                    string userFacingMessage = CultureHelper.Exception("AccountWithIdNotFound", producerId.ToString());

                    throw new NotFoundException(message, userFacingMessage);
                }

                producerName = account.Name + " " + account.Surname;
            }
            else if (producer == Producer.Farm)
            {
                var farm = await DbContext.Farms.FirstOrDefaultAsync(f => f.Id == producerId);

                if (farm == null)
                {
                    string message = $"Farm with Id {producerId} was not found.";
                    string userFacingMessage = CultureHelper.Exception("FarmWithIdNotFound", producerId.ToString());

                    throw new NotFoundException(message, userFacingMessage);
                }

                producerName = farm.Name;
            }

            string fileName = producerName.Replace(' ', '_') + "_" + DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss") + "_" + "products.xlsx";

            return fileName;
        }
    }
    
    class ProductInfo
    {
        public string Name { get; set; }
        public string ArticleNumber { get; set; }
    }
}
