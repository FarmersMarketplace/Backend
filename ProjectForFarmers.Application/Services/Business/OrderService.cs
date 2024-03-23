using AutoMapper;
using FastExcel;
using Geocoding.Google;
using Geocoding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using FarmersMarketplace.Application.DataTransferObjects.Order;
using FarmersMarketplace.Application.Exceptions;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Application.ViewModels.Order;
using FarmersMarketplace.Domain;
using FarmersMarketplace.Application.Helpers;
using InvalidDataException = FarmersMarketplace.Application.Exceptions.InvalidDataException;
using Microsoft.IdentityModel.Tokens;
using Address = FarmersMarketplace.Domain.Address;
using FarmersMarketplace.Application.DataTransferObjects;

namespace FarmersMarketplace.Application.Services.Business
{
    public class OrderService : Service, IOrderService
    {
        private readonly ValidateService Validator;

        public OrderService(IMapper mapper, IApplicationDbContext dbContext, IConfiguration configuration) : base(mapper, dbContext, configuration)
        {
            Validator = new ValidateService(DbContext);
        }

        public async Task<OrderListVm> GetAll(GetOrderListDto getOrderListDto)
        {
            var ordersQuery = DbContext.Orders.Include(o => o.Customer)
                .Where(o => o.CreationDate < getOrderListDto.Cursor
                && o.Producer == getOrderListDto.Producer
                && o.ProducerId == getOrderListDto.ProducerId);

            if (!getOrderListDto.Query.IsNullOrEmpty())
            {
                ordersQuery = ordersQuery.Where(o => o.Number.ToString().Contains(getOrderListDto.Query, StringComparison.OrdinalIgnoreCase)
                && (o.Customer.Name + " " + o.Customer.Surname).Contains(getOrderListDto.Query, StringComparison.OrdinalIgnoreCase));
            }

            if (getOrderListDto.Filter != null)
            {
                ordersQuery = await getOrderListDto.Filter.ApplyFilter(ordersQuery);
            }
            
            var orders = await ordersQuery.OrderByDescending(order => order.CreationDate)
                .Take(getOrderListDto.PageSize)
                .ToListAsync();

            var vm = new OrderListVm
            {
                Orders = new List<OrderLookupVm>(),
                Count = await ordersQuery.CountAsync()
            };

            foreach(var order in orders)
            {
                vm.Orders.Add(Mapper.Map<OrderLookupVm>(order));
            }

            return vm;
        }

        public async Task<(string fileName, byte[] bytes)> ExportToExcel(ExportOrdersDto exportOrdersDto)
        {
            var ordersQuery = DbContext.Orders.Include(o => o.Customer)
                .Where(o => o.Producer == exportOrdersDto.Producer
                && o.ProducerId == exportOrdersDto.ProducerId);

            if(exportOrdersDto.Filter != null)
            {
                ordersQuery = await exportOrdersDto.Filter.ApplyFilter(ordersQuery);
            }

            var orders = ordersQuery.ToList();
            List<Guid> ids = orders.Select(o => o.Id).ToList();

            string fileName = await GetFileName(exportOrdersDto.ProducerId, exportOrdersDto.Producer);
            string filePath = Path.Combine(Configuration["File:Temporary"], fileName);
            string templatePath = Path.Combine(Configuration["File:Temporary"], "template.xlsx");

            using (var fastExcel = new FastExcel.FastExcel(new FileInfo(templatePath), new FileInfo(filePath)))
            {
                var worksheet = new Worksheet();
                var rows = new List<Row>();
                var cells = new List<Cell>();

                cells.Add(new Cell(1, CultureHelper.Property("Id"))); //Id
                cells.Add(new Cell(2, CultureHelper.Property("Number"))); //Номер
                cells.Add(new Cell(3, CultureHelper.Property("OrderDate"))); //Дата замовлення
                cells.Add(new Cell(4, CultureHelper.Property("CustomerName"))); //Ім'я покупця
                cells.Add(new Cell(5, CultureHelper.Property("CustomerEmail"))); //Email покупця
                cells.Add(new Cell(6, CultureHelper.Property("Phone"))); //Телефон
                cells.Add(new Cell(7, CultureHelper.Property("Amount"))); //Сума
                cells.Add(new Cell(8, CultureHelper.Property("PaymentType"))); //Спосіб оплати
                cells.Add(new Cell(9, CultureHelper.Property("Status"))); //Статус

                rows.Add(new Row(1, cells));

                for (int i = 0; i < orders.Count; i++)
                {
                    cells = new List<Cell>();

                    cells.Add(new Cell(1, orders[i].Id.ToString()));
                    cells.Add(new Cell(2, orders[i].Number));
                    cells.Add(new Cell(3, orders[i].CreationDate));
                    cells.Add(new Cell(4, orders[i].Customer.Name + " " + orders[i].Customer.Surname));
                    cells.Add(new Cell(5, orders[i].Customer.Email));
                    cells.Add(new Cell(6, orders[i].Customer.Phone == null ? "" : orders[i].Customer.Phone));
                    cells.Add(new Cell(7, orders[i].TotalPayment));

                    if (orders[i].PaymentType == PaymentType.Online) cells.Add(new Cell(8, CultureHelper.Property("Online"))); //Онлайн
                    else if (orders[i].PaymentType == PaymentType.Cash) cells.Add(new Cell(8, CultureHelper.Property("Cash"))); //Готівка

                    if (orders[i].Status == OrderStatus.New) cells.Add(new Cell(9, "New")); //Нове
                    else if (orders[i].Status == OrderStatus.InProcessing) cells.Add(new Cell(9, CultureHelper.Property("InProcessing"))); //В обробці
                    else if (orders[i].Status == OrderStatus.Collected) cells.Add(new Cell(9, CultureHelper.Property("Collected"))); //Зібрано
                    else if (orders[i].Status == OrderStatus.InDelivery) cells.Add(new Cell(9, CultureHelper.Property("InDelivery"))); //В доставці
                    else if (orders[i].Status == OrderStatus.Completed) cells.Add(new Cell(9, CultureHelper.Property("Completed"))); //Виконано

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
                var account = await DbContext.Sellers.FirstOrDefaultAsync(a => a.Id == producerId);

                if (account == null)
                {
                    string message = $"Account with Id {producerId} was not found.";
                    string userFacingMessage = CultureHelper.Exception("AccountNotFound");

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
                    string userFacingMessage = CultureHelper.Exception("FarmNotFound");

                    throw new NotFoundException(message, userFacingMessage);
                }

                producerName = farm.Name;
            }

            string fileName = producerName.Replace(' ', '_') + "_" + DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss") + "_" + "orders.xlsx";

            return fileName;
        }

        public async Task Duplicate(OrderListDto orderListDto, Guid accountId)
        {
            foreach(var orderId in orderListDto.OrderIds)
            {
                var order = DbContext.Orders.FirstOrDefault(o => o.Id == orderId);

                if (order == null)
                {
                    string message = $"Order with id {orderId} was not found.";
                    string userFacingMessage = CultureHelper.Exception("OrderWithIdNotExist");

                    throw new NotFoundException(message, userFacingMessage);
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

                var deliveryPoint = new Address
                {
                    Id = Guid.NewGuid(),
                    Region = order.DeliveryPoint.Region,
                    Settlement = order.DeliveryPoint.Settlement,
                    District = order.DeliveryPoint.District,
                    Street = order.DeliveryPoint.Street,
                    HouseNumber = order.DeliveryPoint.HouseNumber,
                    PostalCode = order.DeliveryPoint.PostalCode,
                    Note = order.DeliveryPoint.Note,
                    Latitude = order.DeliveryPoint.Latitude,
                    Longitude = order.DeliveryPoint.Longitude,
                };

                DbContext.ProducerAddresses.Add(deliveryPoint);

                var newOrder = new Order
                {
                    Id = newOrderId,
                    Number = order.Number,
                    CreationDate = DateTime.UtcNow,
                    ReceiveDate = order.ReceiveDate,
                    TotalPayment = order.TotalPayment,
                    PaymentType = order.PaymentType,
                    PaymentStatus = order.PaymentStatus,
                    ReceivingType = order.ReceivingType,
                    DeliveryPointId = deliveryPoint.Id,
                    DeliveryPoint = deliveryPoint,
                    Producer = order.Producer,
                    Status = OrderStatus.New,
                    ProducerId = order.ProducerId,
                    CustomerId = order.CustomerId,
                    Customer = order.Customer,
                    Items = items
                };

                await DbContext.Orders.AddAsync(newOrder);
            }

            await DbContext.SaveChangesAsync();
        }

        public async Task Delete(OrderListDto orderListDto, Guid accountId)
        {
            foreach (var orderId in orderListDto.OrderIds)
            {
                var order = DbContext.Orders.FirstOrDefault(o => o.Id == orderId);

                if (order == null)
                {
                    string message = $"Order with id {orderId} was not found.";
                    string userFacingMessage = CultureHelper.Exception("OrderNotExist");

                    throw new NotFoundException(message, userFacingMessage);
                }

                Validator.ValidateProducer(accountId, order.ProducerId, order.Producer);

                DbContext.Orders.Remove(order);
            }

            await DbContext.SaveChangesAsync();
        }

        public async Task<OrderVm> Get(Guid orderId)
        {
            var order = await DbContext.Orders.Include(o => o.Items).FirstOrDefaultAsync();

            if (order == null)
            {
                string message = $"Order with id {orderId} was not found.";
                string userFacingMessage = CultureHelper.Exception("OrderNotExist");

                throw new NotFoundException(message, userFacingMessage);
            }

            var vm = Mapper.Map<OrderVm>(order);
            var items = new List<OrderItemVm>();

            foreach (var item in order.Items)
            {
                var product = await DbContext.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);

                if (product == null)
                {
                    string message = $"Product with id {item.ProductId} was not found.";
                    string userFacingMessage = CultureHelper.Exception("ProductNotExist");

                    throw new NotFoundException(message, userFacingMessage);
                }

                var itemVm = new OrderItemVm
                {
                    Id = item.Id,
                    Name = product.Name,
                    Count = item.Count,
                    PhotoName = product.ImagesNames.Count > 0 ? product.ImagesNames[0] : null,
                    PricePerOne = product.PricePerOne,
                    TotalPrice = product.PricePerOne * item.Count
                };

                items.Add(itemVm);
            }

            vm.Items = items;

            return vm;
        }

        public async Task Update(UpdateOrderDto updateOrderDto, Guid accountId)
        {
            var order = await DbContext.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == updateOrderDto.Id);

            if (order == null)
            {
                string message = $"Order with id {updateOrderDto.Id} was not found.";
                string userFacingMessage = CultureHelper.Exception("OrderNotExist");

                throw new NotFoundException(message, userFacingMessage);
            }

            Validator.ValidateProducer(accountId, order.ProducerId, order.Producer);

            order.ReceiveDate = updateOrderDto.ReceiveDate;
            order.PaymentType = updateOrderDto.PaymentType;
            order.PaymentStatus = updateOrderDto.PaymentStatus;
            order.ReceivingType = updateOrderDto.ReceivingType;
            order.Status = updateOrderDto.Status;

            await UpdateAddress(order.DeliveryPoint, updateOrderDto.DeliveryAddress);
            
            foreach (var item in order.Items)
            {
                var itemDto = updateOrderDto.Items.FirstOrDefault(i => i.Id == item.Id);
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
        }

        private async Task UpdateAddress(Domain.Address address, AddressDto addressDto)
        {
            if (address.Region != addressDto.Region
                || address.District != addressDto.District
                || address.Settlement != addressDto.Settlement
                || address.Street != addressDto.Street
                || address.HouseNumber != addressDto.HouseNumber)
            {
                var coords = await GetCoordinates(addressDto);
                address.Latitude = coords.Latitude;
                address.Longitude = coords.Longitude;
            }

            address.Region = addressDto.Region;
            address.District = addressDto.District;
            address.Settlement = addressDto.Settlement;
            address.Street = addressDto.Street;
            address.HouseNumber = addressDto.HouseNumber;
            address.PostalCode = addressDto.PostalCode;
            address.Note = addressDto.Note;
        }

        private async Task<Location> GetCoordinates(AddressDto dto)
        {
            IGeocoder geocoder = new GoogleGeocoder() { ApiKey = Configuration["Geocoding:Apikey"] };
            var request = await geocoder.GeocodeAsync($"{dto.Region} oblast, {dto.District} district, {dto.Settlement} street {dto.Street}, {dto.HouseNumber}, Ukraine");
            var coords = request.FirstOrDefault().Coordinates;
            return coords;
        }

        public async Task AddOrderItem(AddOrderItemDto addOrderItemDto, Guid accountId)
        {
            var order = await DbContext.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == addOrderItemDto.OrderId);

            if (order == null)
            {
                string message = $"Order with id {addOrderItemDto.OrderId} was not found.";
                string userFacingMessage = CultureHelper.Exception("OrderNotExist");

                throw new NotFoundException(message, userFacingMessage);
            }

            Validator.ValidateProducer(accountId, order.ProducerId, order.Producer);

            var product = await DbContext.Products.FirstOrDefaultAsync(p => p.Id == addOrderItemDto.ProductId);

            if (product == null)
            {
                string message = $"Product with id {addOrderItemDto.ProductId} was not found.";
                string userFacingMessage = CultureHelper.Exception("ProductNotExist");

                throw new NotFoundException(message, userFacingMessage);
            }

            if (product.CreationDate > order.ReceiveDate)
            {
                string message = "Creation date of product cannot be later than receive date.";
                string userFacingMessage = CultureHelper.Exception("ProductCreationDateIsLaterThanReceiveDate");

                throw new InvalidDataException(message, userFacingMessage);
            }

            var item = new OrderItem
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                OrderId = order.Id,
                Count = addOrderItemDto.Count,
                TotalPrice = product.PricePerOne * addOrderItemDto.Count
            };

            order.Items.Add(item);

            await DbContext.SaveChangesAsync();
        }
    }

}
