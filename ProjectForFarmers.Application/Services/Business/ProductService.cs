using AutoMapper;
using FarmersMarketplace.Application.DataTransferObjects.Product;
using FarmersMarketplace.Application.Exceptions;
using FarmersMarketplace.Application.Filters;
using FarmersMarketplace.Application.Helpers;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Application.ViewModels.Account;
using FarmersMarketplace.Application.ViewModels.Product;
using FarmersMarketplace.Application.ViewModels.Subcategory;
using FarmersMarketplace.Domain;
using FarmersMarketplace.Domain.Accounts;
using FastExcel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using InvalidDataException = FarmersMarketplace.Application.Exceptions.InvalidDataException;

namespace FarmersMarketplace.Application.Services.Business
{
    public class ProductService : Service, IProductService
    {
        private readonly FileHelper FileHelper;
        private readonly string ProductsImagesFolder;
        private readonly string DocumentsFolder;
        private readonly ValidateService Validator;
        private readonly ICacheProvider<Product> CacheProvider;
        private readonly ISearchSynchronizer<Product> SearchSynchronizer;

        public ProductService(IMapper mapper, IApplicationDbContext dbContext, IConfiguration configuration, ICacheProvider<Product> cacheProvider, ISearchSynchronizer<Product> searchSynchronizer) : base(mapper, dbContext, configuration)
        {
            FileHelper = new FileHelper();
            ProductsImagesFolder = Configuration["File:Images:Product"];
            DocumentsFolder = Configuration["File:Documents"];
            Validator = new ValidateService(DbContext);
            CacheProvider = cacheProvider;
            SearchSynchronizer = searchSynchronizer;
        }

        public async Task Create(CreateProductDto dto)
        {
            var product = Mapper.Map<Product>(dto);

            var category = await DbContext.Categories.FirstOrDefaultAsync(c => c.Id == product.CategoryId);

            if (category == null)
            {
                string message = $"Category with Id {product.CategoryId} was not found.";
                throw new NotFoundException(message, "CategoryNotFound", product.CategoryId.ToString());
            }

            var subcategory = await DbContext.Subcategories.FirstOrDefaultAsync(c => c.Id == product.SubcategoryId);

            if (subcategory == null)
            {
                string message = $"Subcategory with Id {product.SubcategoryId} was not found.";
                throw new NotFoundException(message, "SubcategoryNotFound", product.SubcategoryId.ToString());
            }
            else if(subcategory.CategoryId != category.Id)
            {
                string message = $"Subcategory does not belong to the category.";
                throw new InvalidDataException(message, "SubcategoryNotBelongsToCategory", product.SubcategoryId.ToString(), product.CategoryId.ToString());
            }

            product.ArticleNumber = GenerateArticleNumber();

            if(dto.Images.Count > 0)
            {
                product.ImagesNames = await FileHelper.SaveImages(dto.Images, ProductsImagesFolder);
            }
            else if (dto.UseSubcategoryImage)
            {
                product.ImagesNames = new List<string> { subcategory.ImageName };
            }
            else
            {
                product.ImagesNames = new List<string>();
            }
            
            if(dto.Documents != null)
            {
                product.DocumentsNames = await FileHelper.SaveFiles(dto.Documents, DocumentsFolder);
            }
            else
            {
                product.DocumentsNames = new List<string>();
            }
            
            DbContext.Products.Add(product);
            await DbContext.SaveChangesAsync();
            await SearchSynchronizer.Create(product);
        }

        public async Task Delete(ProductListDto dto, Guid accountId)
        {
            foreach (var productId in dto.Products)
            {
                var product = await DbContext.Products.FirstAsync(p => p.Id == productId);

                if (product == null)
                {
                    string message = $"Product with Id {productId} was not found.";
                    throw new NotFoundException(message, "ProductNotFound");
                }
                Validator.ValidateProducer(accountId, product.ProducerId, product.Producer);

                bool hasOrdersWithProduct = await DbContext.OrdersItems.AnyAsync(oi => oi.ProductId == productId);

                if (hasOrdersWithProduct)
                {
                    string message = $"Product with Id {productId} is used in existing orders.";
                    throw new NotFoundException(message, "ProductIsUsed");
                }
                else
                {
                    await FileHelper.DeleteFiles(product.ImagesNames, ProductsImagesFolder);
                    await FileHelper.DeleteFiles(product.DocumentsNames, DocumentsFolder);
                    DbContext.Products.Remove(product);
                    await DbContext.SaveChangesAsync();
                    await SearchSynchronizer.Delete(product.Id);
                    await CacheProvider.Delete(product.Id);
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

        public async Task<ProducerProductVm> GetForProducer(Guid productId)
        {
            if (CacheProvider.Exists(productId))
            {
                var p = await CacheProvider.Get(productId);
                return Mapper.Map<ProducerProductVm>(p);
            }

            var product = await DbContext.Products.Include(p => p.Category)
                .Include(p => p.Subcategory)
                .FirstAsync(p => p.Id == productId);

            if (product == null)
            {
                string message = $"Product with Id {productId} was not found.";
                throw new NotFoundException(message, "ProductNotFound");
            }

            await CacheProvider.Set(product);
            var vm = Mapper.Map<ProducerProductVm>(product);

            return vm;
        }

        

        public async Task Update(UpdateProductDto dto, Guid accountId)
        {
            var category = await DbContext.Categories.FirstOrDefaultAsync(c => c.Id == dto.CategoryId);
            if (category == null)
            {
                string message = $"Category with Id {dto.CategoryId} was not found.";
                throw new NotFoundException(message, "CategoryNotFound");
            }

            var subcategory = await DbContext.Subcategories.FirstOrDefaultAsync(c => c.Id == dto.SubcategoryId);
            if (subcategory == null)
            {
                string message = $"Subcategory with Id {dto.SubcategoryId} was not found.";
                throw new NotFoundException(message, "SubcategoryNotFound");
            }
            else if (subcategory.CategoryId != category.Id)
            {
                string message = $"Subcategory with Id {dto.SubcategoryId} does not belong to the category with Id {dto.CategoryId}.";
                throw new InvalidDataException(message, "SubcategoryNotBelongsToCategory");
            }

            var product = await DbContext.Products.FirstAsync(p => p.Id == dto.Id);

            if (product == null)
            {
                string message = $"Product with Id {dto.Id} was not found.";
                throw new NotFoundException(message, "ProductNotFound");
            }
            Validator.ValidateProducer(accountId, product.ProducerId, product.Producer);

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.CategoryId = dto.CategoryId;
            product.SubcategoryId = dto.SubcategoryId;
            product.PackagingType = dto.PackagingType;
            product.Status = dto.Status;
            product.UnitOfMeasurement = dto.UnitOfMeasurement;
            product.PricePerOne = dto.PricePerOne;
            product.MinPurchaseQuantity = dto.MinPurchaseQuantity;
            product.Count = dto.Count;
            product.ExpirationDays = dto.ExpirationDays;
            product.CreationDate = dto.CreationDate;

            if (dto.Images == null) dto.Images = new List<IFormFile>();
            if (dto.Documents == null) dto.Documents = new List<IFormFile>();

            if (product.ImagesNames == null) product.ImagesNames = new List<string>();
            if (product.DocumentsNames == null) product.DocumentsNames = new List<string>();

            foreach (var imageName in product.ImagesNames.ToList())
            {
                if (!dto.Images.Any(file => file.FileName == imageName))
                {
                    FileHelper.DeleteFile(imageName, ProductsImagesFolder);
                    product.ImagesNames.Remove(imageName);
                }
            }

            foreach (var documentName in product.DocumentsNames.ToList())
            {
                if (!dto.Documents.Any(file => file.FileName == documentName))
                {
                    FileHelper.DeleteFile(documentName, DocumentsFolder);
                    product.DocumentsNames.Remove(documentName);
                }
            }

            foreach (var newImage in dto.Images)
            {
                if (!product.ImagesNames.Contains(newImage.FileName))
                {
                    string imageName = await FileHelper.SaveFile(newImage, ProductsImagesFolder);
                    product.ImagesNames.Add(imageName);
                }
            }

            foreach (var newDocument in dto.Documents)
            {
                if (!product.DocumentsNames.Contains(newDocument.FileName))
                {
                    string documentName = await FileHelper.SaveFile(newDocument, DocumentsFolder);
                    product.DocumentsNames.Add(documentName);
                }
            }

            await DbContext.SaveChangesAsync();
            await SearchSynchronizer.Update(product);
            await CacheProvider.Update(product);
        }

        public async Task Duplicate(ProductListDto dto, Guid accountId)
        {
            foreach(var productId in dto.Products)
            {
                var product = await DbContext.Products.FirstOrDefaultAsync(p => p.Id == productId);

                if (product == null)
                {
                    string message = $"Product with Id {productId} was not found.";
                    throw new NotFoundException(message, "ProductNotFound");
                }
                Validator.ValidateProducer(accountId, product.ProducerId, product.Producer);

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
                    ExpirationDays = product.ExpirationDays,
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

        public async Task<ProducerProductFilterData> GetProducerProductFilterData(Producer producer, Guid producerId)
        {
            var unitsOfMeasurement = await DbContext.Products.Where(p => p.Producer == producer
                && p.ProducerId == producerId)
                .Select(p => p.UnitOfMeasurement)
                .ToListAsync();

            var vm = new ProducerProductFilterData();

            if (producer == Producer.Seller)
            {
                var seller = await DbContext.Sellers.FirstOrDefaultAsync(s => s.Id == producerId);

                if (seller == null)
                {
                    string message = $"Account with Id {producerId} was not found.";
                    throw new NotFoundException(message, "AccountNotFound");
                }

                foreach (var subcategoryId in seller.Subcategories)
                {
                    var subcategory = DbContext.Subcategories.FirstOrDefault(c => c.Id == subcategoryId);
                    if (subcategory == null)
                    {
                        string message = $"Subcategory with Id {subcategoryId} was not found.";
                        throw new NotFoundException(message, "SubcategoryNotFound");
                    }

                    vm.Subcategories.Add(new SubcategoryVm(subcategory.Id, subcategory.Name, subcategory.CategoryId));
                }
            }
            else if (producer == Producer.Farm)
            {
                var farm = await DbContext.Farms.FirstOrDefaultAsync(f => f.Id == producerId);

                if (farm == null)
                {
                    string message = $"Farm with Id {producerId} was not found.";
                    throw new NotFoundException(message, "FarmNotFound");
                }

                foreach (var subcategoryId in farm.Subcategories)
                {
                    var subcategory = DbContext.Subcategories.FirstOrDefault(c => c.Id == subcategoryId);
                    if (subcategory == null)
                    {
                        string message = $"Subcategory with Id {subcategoryId} was not found.";
                        throw new NotFoundException(message, "SubcategoryNotFound");
                    }

                    vm.Subcategories.Add(new SubcategoryVm(subcategory.Id, subcategory.Name, subcategory.CategoryId));
                }
            }
            else
            {
                throw new NotImplementedException();
            }
            
            vm.UnitsOfMeasurement = unitsOfMeasurement;

            return vm;
        }

        public async Task<(string fileName, byte[] bytes)> ExportToExcel(ExportProductsDto dto)
        {
            var productsQuery = DbContext.Products.Include(p => p.Category)
                .Include(p => p.Subcategory)
                .Where(p => p.ProducerId == dto.ProducerId 
                && p.Producer == dto.Producer);

            if(dto.Filter != null)
            {
                productsQuery = await ApplyFilter(productsQuery, dto.Filter);
            }

            List<ProducerProductLookupVm> products = await productsQuery.Select(p => Mapper.Map<ProducerProductLookupVm>(p)).ToListAsync();

            string fileName = await GetFileName(dto.ProducerId, dto.Producer);
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

        public async Task<IQueryable<Product>> ApplyFilter(IQueryable<Product> query, ProducerProductFilter filter)
        {
            return query
                    .Where(p =>
                        (!filter.Subcategories.Any() || filter.Subcategories.Contains(p.SubcategoryId)) &&
                        (!filter.MinCreationDate.HasValue || p.CreationDate >= filter.MinCreationDate) &&
                        (!filter.MaxCreationDate.HasValue || p.CreationDate <= filter.MaxCreationDate) &&
                        (!filter.UnitsOfMeasurement.Any() || filter.UnitsOfMeasurement.Contains(p.UnitOfMeasurement)) &&
                        (!filter.MinRest.HasValue || p.Count >= filter.MinRest) &&
                        (!filter.MaxRest.HasValue || p.Count <= filter.MaxRest));
        }
        private async Task<string> GetFileName(Guid producerId, Producer producer)
        {
            string producerName = "";

            if (producer == Producer.Seller)
            {
                var account = await DbContext.Sellers.FirstOrDefaultAsync(a => a.Id == producerId);

                if (account == null)
                {
                    string message = $"Account with Id {producerId} was not found.";
                    throw new NotFoundException(message, "AccountNotFound");
                }

                producerName = account.Name + " " + account.Surname;
            }
            else if (producer == Producer.Farm)
            {
                var farm = await DbContext.Farms.FirstOrDefaultAsync(f => f.Id == producerId);

                if (farm == null)
                {
                    string message = $"Farm with Id {producerId} was not found.";
                    throw new NotFoundException(message, "FarmNotFound");
                }

                producerName = farm.Name;
            }

            string fileName = producerName.Replace(' ', '_') + "_" + DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss") + "_" + "products.xlsx";

            return fileName;
        }

        public async Task ChangeStatus(ProductListDto dto, ProductStatus status, Guid accountId)
        {
            foreach (var productId in dto.Products)
            {
                var product = await DbContext.Products.FirstAsync(p => p.Id == productId);

                if (product == null)
                {
                    string message = $"Product with Id {productId} was not found.";
                    throw new NotFoundException(message, "ProductNotFound");
                }

                Validator.ValidateProducer(accountId, product.ProducerId, product.Producer);

                product.Status = status;
            }
            await DbContext.SaveChangesAsync();
        }
    }
}
