using AutoMapper;
using FarmersMarketplace.Application.DataTransferObjects;
using FarmersMarketplace.Application.DataTransferObjects.Order;
using FarmersMarketplace.Application.Exceptions;
using FarmersMarketplace.Application.Filters;
using FarmersMarketplace.Application.Helpers;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Application.ViewModels;
using FarmersMarketplace.Application.ViewModels.Order;
using FarmersMarketplace.Domain;
using FarmersMarketplace.Domain.Orders;
using FarmersMarketplace.Domain.Payment;
using FastExcel;
using Geocoding;
using Geocoding.Google;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Address = FarmersMarketplace.Domain.Address;
using InvalidDataException = FarmersMarketplace.Application.Exceptions.InvalidDataException;

namespace FarmersMarketplace.Application.Services.Business
{
    public class OrderService : Service, IOrderService
    {
        private readonly ValidateService Validator;
        private readonly ICacheProvider<Order> CacheProvider;
        private readonly ISearchSynchronizer<Order> SearchSynchronizer;
        private readonly LiqpayService LiqpayService;

        public OrderService(IMapper mapper, IApplicationDbContext dbContext, IConfiguration configuration, ICacheProvider<Order> cacheProvider, ISearchSynchronizer<Order> searchSynchronizer) : base(mapper, dbContext, configuration)
        {
            Validator = new ValidateService(DbContext);
            CacheProvider = cacheProvider;
            SearchSynchronizer = searchSynchronizer;
            LiqpayService = new LiqpayService(configuration);
        }

        public async Task<(string fileName, byte[] bytes)> ExportToExcel(ExportOrdersDto dto)
        {
            var ordersQuery = DbContext.Orders
                .Where(o => o.Producer == dto.Producer
                && o.ProducerId == dto.ProducerId);

            if(dto.Filter != null)
            {
                ordersQuery = await ApplyFilter(ordersQuery, dto.Filter);
            }

            var orders = ordersQuery.ToList();
            List<Guid> ids = orders.Select(o => o.Id).ToList();

            string fileName = await GetFileName(dto.ProducerId, dto.Producer);
            string filePath = Path.Combine(Configuration["File:Temporary"], fileName);
            string templatePath = Path.Combine(Configuration["File:Temporary"], "template.xlsx");

            using (var fastExcel = new FastExcel.FastExcel(new FileInfo(templatePath), new FileInfo(filePath)))
            {
                var worksheet = new Worksheet();
                var rows = new List<Row>();
                var cells = new List<Cell>();

                cells.Add(new Cell(1, CultureHelper.Property("Id"))); 
                cells.Add(new Cell(2, CultureHelper.Property("Number"))); 
                cells.Add(new Cell(3, CultureHelper.Property("OrderDate"))); 
                cells.Add(new Cell(4, CultureHelper.Property("CustomerName")));
                cells.Add(new Cell(5, CultureHelper.Property("Email")));
                cells.Add(new Cell(6, CultureHelper.Property("Phone"))); 
                cells.Add(new Cell(7, CultureHelper.Property("AdditionalPhone")));
                cells.Add(new Cell(8, CultureHelper.Property("Amount"))); 
                cells.Add(new Cell(9, CultureHelper.Property("PaymentType"))); 
                cells.Add(new Cell(10, CultureHelper.Property("Status"))); 

                rows.Add(new Row(1, cells));

                for (int i = 0; i < orders.Count; i++)
                {
                    cells = new List<Cell>();

                    cells.Add(new Cell(1, orders[i].Id.ToString()));
                    cells.Add(new Cell(2, orders[i].Number));
                    cells.Add(new Cell(3, orders[i].CreationDate));
                    cells.Add(new Cell(4, orders[i].CustomerName + " " + orders[i].CustomerSurname));
                    cells.Add(new Cell(5, orders[i].Customer.Email));
                    cells.Add(new Cell(6, orders[i].CustomerPhone == null ? "" : orders[i].CustomerPhone));
                    cells.Add(new Cell(7, orders[i].CustomerAdditionalPhone == null ? "" : orders[i].CustomerAdditionalPhone));
                    cells.Add(new Cell(8, orders[i].TotalPayment));

                    if (orders[i].PaymentType == PaymentType.Online) cells.Add(new Cell(9, CultureHelper.Property("Online")));
                    else if (orders[i].PaymentType == PaymentType.Cash) cells.Add(new Cell(9, CultureHelper.Property("Cash")));

                    if (orders[i].Status == OrderStatus.New) cells.Add(new Cell(10, "New"));
                    else if (orders[i].Status == OrderStatus.InProcessing) cells.Add(new Cell(10, CultureHelper.Property("InProcessing")));
                    else if (orders[i].Status == OrderStatus.Collected) cells.Add(new Cell(10, CultureHelper.Property("Collected")));
                    else if (orders[i].Status == OrderStatus.InDelivery) cells.Add(new Cell(10, CultureHelper.Property("InDelivery")));
                    else if (orders[i].Status == OrderStatus.Completed) cells.Add(new Cell(10, CultureHelper.Property("Completed")));

                    rows.Add(new Row(i + 2, cells));
                }

                worksheet.Rows = rows;
                fastExcel.Write(worksheet, "sheet1");
            }

            byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            System.IO.File.Delete(filePath);

            return (fileName, fileBytes);
        }

        public async Task<IQueryable<Order>> ApplyFilter(IQueryable<Order> query, ProducerOrderFilter filter)
        {
            return query.Where(o =>
                (filter.Statuses == null || !filter.Statuses.Any() || filter.Statuses.Contains(o.Status)) &&
                (!filter.StartDate.HasValue || o.CreationDate >= filter.StartDate) &&
                (!filter.EndDate.HasValue || o.CreationDate <= filter.EndDate) &&
                (filter.PaymentTypes == null || !filter.PaymentTypes.Any() || filter.PaymentTypes.Contains(o.PaymentType)));
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

            string fileName = producerName.Replace(' ', '_') + "_" + DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss") + "_" + "orders.xlsx";

            return fileName;
        }

        public async Task Duplicate(OrderListDto dto, Guid accountId)
        {
            foreach(var orderId in dto.OrderIds)
            {
                var order = DbContext.Orders.FirstOrDefault(o => o.Id == orderId);

                if (order == null)
                {
                    string message = $"Order with id {orderId} was not found.";
                    throw new NotFoundException(message, "OrderWithIdNotExist");
                }
                Validator.ValidateProducer(accountId, order.ProducerId, order.Producer);

                var newOrderId = Guid.NewGuid();
                var items = new List<OrderItem>();

                foreach(var item  in order.Items)
                {
                    var newItem = new OrderItem
                    {
                        Id = Guid.NewGuid(),
                        ProductId = item.ProductId,
                        OrderId = newOrderId,
                        Count = item.Count,
                        TotalPrice = item.TotalPrice
                    };
                    items.Add(newItem);
                }

                var deliveryPoint = new CustomerAddress
                {
                    Id = Guid.NewGuid(),
                    Region = order.DeliveryPoint.Region,
                    Settlement = order.DeliveryPoint.Settlement,
                    District = order.DeliveryPoint.District,
                    Street = order.DeliveryPoint.Street,
                    HouseNumber = order.DeliveryPoint.HouseNumber,
                    PostalCode = order.DeliveryPoint.PostalCode,
                    Note = order.DeliveryPoint.Note,
                    Apartment = order.DeliveryPoint.Apartment,
                    Latitude = order.DeliveryPoint.Latitude,
                    Longitude = order.DeliveryPoint.Longitude,
                };

                DbContext.CustomerAddresses.Add(deliveryPoint);

                var newOrder = new Order
                {
                    Id = newOrderId,
                    Number = order.Number,
                    CreationDate = DateTime.UtcNow,
                    ReceiveDate = order.ReceiveDate,
                    TotalPayment = order.TotalPayment,
                    PaymentType = order.PaymentType,
                    PaymentStatus = order.PaymentStatus,
                    ReceivingMethod = order.ReceivingMethod,
                    DeliveryPointId = deliveryPoint.Id,
                    DeliveryPoint = deliveryPoint,
                    Producer = order.Producer,
                    Status = OrderStatus.New,
                    ProducerId = order.ProducerId,
                    CustomerId = order.CustomerId,
                    CustomerName = order.CustomerName,
                    CustomerSurname = order.CustomerSurname,
                    CustomerPhone = order.CustomerPhone,
                    CustomerAdditionalPhone = order.CustomerAdditionalPhone,
                    Items = items
                };

                await DbContext.Orders.AddAsync(newOrder);
                await SearchSynchronizer.Create(newOrder);
            }

            await DbContext.SaveChangesAsync();
        }

        public async Task Delete(OrderListDto dto, Guid accountId)
        {
            foreach (var orderId in dto.OrderIds)
            {
                var order = DbContext.Orders.FirstOrDefault(o => o.Id == orderId);

                if (order == null)
                {
                    string message = $"Order with id {orderId} was not found.";
                    throw new NotFoundException(message, "OrderNotExist");
                }

                Validator.ValidateProducer(accountId, order.ProducerId, order.Producer);

                DbContext.Orders.Remove(order);
                await SearchSynchronizer.Delete(order.Id);
                await CacheProvider.Delete(order.Id);
            }

            await DbContext.SaveChangesAsync();
        }

        public async Task<OrderForProducerVm> GetForProducer(Guid orderId)
        {
            if (CacheProvider.Exists(orderId))
            {
                var s = await CacheProvider.Get(orderId);
                return Mapper.Map<OrderForProducerVm>(s);
            }

            var order = await DbContext.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                string message = $"Order with id {orderId} was not found.";
                throw new NotFoundException(message, "OrderNotExist");
            }

            var vm = Mapper.Map<OrderForProducerVm>(order);
            var items = new List<OrderItemVm>();

            foreach (var item in order.Items)
            {
                var product = await DbContext.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);

                if (product == null)
                {
                    string message = $"Product with id {item.ProductId} was not found.";
                    throw new NotFoundException(message, "ProductNotExist");
                }

                var itemVm = new OrderItemVm
                {
                    Id = item.Id,
                    Name = product.Name,
                    Count = item.Count,
                    ImageName = product.ImagesNames.Count > 0 ? product.ImagesNames[0] : null,
                    PricePerOne = item.PricePerOne,
                    TotalPrice = product.PricePerOne * item.Count,
                    ArticleNumber = product.ArticleNumber,
                    UnitOfMeasurement = product.UnitOfMeasurement,
                };

                items.Add(itemVm);
            }

            vm.Items = items;
            await CacheProvider.Set(order);
            return vm;
        }

        public async Task Update(UpdateOrderDto dto, Guid accountId)
        {
            var order = await DbContext.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == dto.Id);

            if (order == null)
            {
                string message = $"Order with id {dto.Id} was not found.";
                throw new NotFoundException(message, "OrderNotExist");
            }

            Validator.ValidateProducer(accountId, order.ProducerId, order.Producer);

            order.ReceiveDate = dto.ReceiveDate;
            order.PaymentType = dto.PaymentType;
            order.PaymentStatus = dto.PaymentStatus;
            order.ReceivingMethod = dto.ReceivingType;
            order.Status = dto.Status;

            await UpdateAddress(order.DeliveryPoint, dto.DeliveryAddress);
            
            foreach (var item in order.Items)
            {
                var itemDto = dto.Items.FirstOrDefault(i => i.Id == item.Id);
                if(itemDto == null)
                {
                    order.Items.Remove(item);
                }
                else
                {
                    item.Count = itemDto.Count;
                }
            }

            await DbContext.SaveChangesAsync();
            await CacheProvider.Update(order);
            await SearchSynchronizer.Update(order);
        }

        private async Task UpdateAddress(Address address, AddressDto dto)
        {
            if (address.Region != dto.Region
                || address.District != dto.District
                || address.Settlement != dto.Settlement
                || address.Street != dto.Street
                || address.HouseNumber != dto.HouseNumber)
            {
                var coords = await GetCoordinates(dto);
                address.Latitude = coords.Latitude;
                address.Longitude = coords.Longitude;
            }

            address.Region = dto.Region;
            address.District = dto.District;
            address.Settlement = dto.Settlement;
            address.Street = dto.Street;
            address.HouseNumber = dto.HouseNumber;
            address.PostalCode = dto.PostalCode;
            address.Note = dto.Note;
        }

        private async Task<Geocoding.Location> GetCoordinates(AddressDto dto)
        {
            IGeocoder geocoder = new GoogleGeocoder() { ApiKey = Configuration["Geocoding:Apikey"] };
            var request = await geocoder.GeocodeAsync($"{dto.Region} oblast, {dto.District} district, {dto.Settlement} street {dto.Street}, {dto.HouseNumber}, Ukraine");
            var coords = request.FirstOrDefault().Coordinates;
            return coords;
        }

        public async Task AddOrderItem(AddOrderItemDto dto, Guid accountId)
        {
            var order = await DbContext.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == dto.OrderId);

            if (order == null)
            {
                string message = $"Order with id {dto.OrderId} was not found.";
                throw new NotFoundException(message, "OrderNotExist");
            }

            Validator.ValidateProducer(accountId, order.ProducerId, order.Producer);

            var product = await DbContext.Products.FirstOrDefaultAsync(p => p.Id == dto.ProductId);

            if (product == null)
            {
                string message = $"Product with id {dto.ProductId} was not found.";
                throw new NotFoundException(message, "ProductNotExist");
            }

            if (product.CreationDate > order.ReceiveDate)
            {
                string message = "Creation date of product cannot be later than receive date.";
                throw new InvalidDataException(message, "ProductCreationDateIsLaterThanReceiveDate");
            }

            var item = new OrderItem
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                OrderId = order.Id,
                Count = dto.Count,
                TotalPrice = product.PricePerOne * dto.Count
            };

            order.Items.Add(item);

            await DbContext.SaveChangesAsync();
        }

        public async Task ChangeStatus(OrderListDto dto, OrderStatus status, Guid accountId)
        {
            foreach (var orderId in dto.OrderIds)
            {
                var order = DbContext.Orders.FirstOrDefault(o => o.Id == orderId);

                if (order == null)
                {
                    string message = $"Order with id {orderId} was not found.";
                    throw new NotFoundException(message, "OrderNotExist");
                }

                Validator.ValidateProducer(accountId, order.ProducerId, order.Producer);

                order.Status = status;
                await CacheProvider.Update(order);
                await SearchSynchronizer.Update(order);
            }

            await DbContext.SaveChangesAsync();
        }

        public async Task<OrderForCustomerVm> GetForCustomer(Guid orderId)
        {
            if (CacheProvider.Exists(orderId))
            {
                var o = await CacheProvider.Get(orderId);
                return Mapper.Map<OrderForCustomerVm>(o);
            }

            var order = await DbContext.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                string message = $"Order with id {orderId} was not found.";
                throw new NotFoundException(message, "OrderNotExist");
            }

            var vm = await FormOrderForCustomer(order);
            var items = new List<OrderItemVm>();

            foreach (var item in order.Items)
            {
                var product = await DbContext.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);

                if (product == null)
                {
                    string message = $"Product with id {item.ProductId} was not found.";
                    throw new NotFoundException(message, "ProductNotExist");
                }

                var itemVm = new OrderItemVm
                {
                    Id = item.Id,
                    Name = product.Name,
                    Count = item.Count,
                    ImageName = product.ImagesNames.Count > 0 ? product.ImagesNames[0] : null,
                    PricePerOne = item.PricePerOne,
                    TotalPrice = product.PricePerOne * item.Count,
                    ArticleNumber = product.ArticleNumber,
                    UnitOfMeasurement = product.UnitOfMeasurement,
                };

                items.Add(itemVm);
            }

            vm.Items = items;
            await CacheProvider.Set(order);
            return vm;
        }

        private async Task<OrderForCustomerVm> FormOrderForCustomer(Order order)
        {
            var vm = Mapper.Map<OrderForCustomerVm>(order);

            if(order.Producer == Producer.Farm)
            {
                var farm = await DbContext.Farms.Include(f => f.Address)
                    .FirstOrDefaultAsync(f => f.Id == order.ProducerId);

                if (farm == null)
                {
                    string message = $"Farm with Id {order.ProducerId} was not found.";
                    throw new NotFoundException(message, "FarmNotFound");
                }

                vm.ProducerName = farm.Name;
                vm.ProducerAddress = Mapper.Map<AddressVm>(farm.Address);
                vm.ProducerImageName = farm.ImagesNames.Count > 0 ? farm.ImagesNames[0] : "";
            }
            else if(order.Producer == Producer.Seller)
            {
                var seller = await DbContext.Sellers.Include(s => s.Address)
                    .FirstOrDefaultAsync(a => a.Id == order.ProducerId);

                if (seller == null)
                {
                    string message = $"Account with Id {order.ProducerId} was not found.";
                    throw new NotFoundException(message, "AccountNotFound");
                }

                vm.ProducerName = seller.Name + " " + seller.Surname;
                vm.ProducerImageName = seller.ImagesNames.Count > 0 ? seller.ImagesNames[0] : "";
                vm.ProducerAddress = Mapper.Map<AddressVm>(seller.Address);
            }

            return vm;
        }

        public async Task Create(CreateOrderDto dto)
        {
            var items = await DbContext.OrderItems.Where(i => dto.OrderItemsIds.Contains(i.Id)).ToListAsync();

            var order = new Order
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTime.UtcNow,
                ReceiveDate = dto.ReceiveDate,
                TotalPayment = items.Sum(i => i.TotalPrice),
                PaymentType = dto.PaymentType,
                ReceivingMethod = dto.ReceivingMethod,
                Producer = dto.Producer,
                Status = OrderStatus.New,
                ProducerId = dto.ProducerId,
                CustomerId = dto.CustomerId,
                CustomerName = dto.CustomerName,
                CustomerSurname = dto.CustomerSurname,
                CustomerPhone = dto.CustomerPhone,
                CustomerAdditionalPhone = dto.CustomerAdditionalPhone,
                Items = items,
            };

            if(dto.ReceivingMethod == ReceivingMethod.Delivery)
            {
                var address = Mapper.Map<CustomerAddress>(dto.DeliveryPoint);
                order.DeliveryPoint = address;
                string receiverCard = "";

                if (order.Producer == Producer.Farm)
                {
                    var farm = await DbContext.Farms.Include(f => f.PaymentData)
                        .FirstOrDefaultAsync(f => f.Id == order.ProducerId);

                    if (farm == null)
                    {
                        string message = $"Farm with Id {order.ProducerId} was not found.";
                        throw new NotFoundException(message, "FarmNotFound");
                    }

                    receiverCard = farm.PaymentData.CardNumber;
                }
                else if (order.Producer == Producer.Seller)
                {
                    var seller = await DbContext.Sellers.Include(s => s.Address)
                        .FirstOrDefaultAsync(a => a.Id == order.ProducerId);

                    if (seller == null)
                    {
                        string message = $"Account with Id {order.ProducerId} was not found.";
                        throw new NotFoundException(message, "AccountNotFound");
                    }

                    receiverCard = seller.PaymentData.CardNumber;
                }

                await LiqpayService.Pay(dto.PaymentData, order.TotalPayment, order.Id, receiverCard);
                order.PaymentStatus = PaymentStatus.Paid;
            }
            else if(dto.ReceivingMethod == ReceivingMethod.SelfPickUp)
            {
                order.PaymentStatus = PaymentStatus.Unpaid;
            }

            await DbContext.Orders.AddAsync(order);
            await DbContext.SaveChangesAsync();
            await SearchSynchronizer.Create(order);
        }
    }

}
