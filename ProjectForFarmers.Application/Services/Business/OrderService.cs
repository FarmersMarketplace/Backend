using AutoMapper;
using FastExcel;
using Geocoding.Google;
using Geocoding;
using Hangfire.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectForFarmers.Application.DataTransferObjects.Farm;
using ProjectForFarmers.Application.DataTransferObjects.Order;
using ProjectForFarmers.Application.Exceptions;
using ProjectForFarmers.Application.Filters;
using ProjectForFarmers.Application.Interfaces;
using ProjectForFarmers.Application.ViewModels.Order;
using ProjectForFarmers.Domain;

namespace ProjectForFarmers.Application.Services.Business
{
    public class OrderService : Service, IOrderService
    {
        private readonly StatisticService StatisticService;

        public OrderService(IMapper mapper, IApplicationDbContext dbContext, IConfiguration configuration) : base(mapper, dbContext, configuration)
        {
            StatisticService = new StatisticService(dbContext);
        }

        public async Task<LoadDashboardVm> LoadDashboard(Guid producerId, Producer producer)
        {
            var statistics = DbContext.MonthesStatistics.Where(m => m.ProducerId == producerId
                && m.Producer == producer).ToList();

            var currentMonthStatistic = await StatisticService.CalculateStatisticForMonth(producerId, producer, DateTimeOffset.Now);
            statistics.Add(currentMonthStatistic);

            var statisticsVm = statistics.Select(s => new MonthStatisticLookupVm
            {
                Id = s.Id,
                StartDate = s.StartDate,
                EndDate = s.EndDate
            }).ToList();

            var currentMonthDashboardVm = Mapper.Map<DashboardVm>(currentMonthStatistic);
            var customerWithHighestPayment = await DbContext.Accounts.FirstOrDefaultAsync(a => a.Id == currentMonthStatistic.CustomerWithHighestPaymentId);

            if (customerWithHighestPayment != null) throw new NotFoundException($"Customer with highest payment was not found(account id: {currentMonthStatistic.CustomerWithHighestPaymentId}).");

            currentMonthDashboardVm.CustomerWithHighestPaymentName = $"{customerWithHighestPayment.Surname} {customerWithHighestPayment.Name}";

            var vm = new LoadDashboardVm
            {
                DateRanges = statisticsVm,
                CurrentMonthDashboard = currentMonthDashboardVm
            };

            return vm;
        }

        public async Task<OrderListVm> GetAll(Guid producerId, Producer producer, OrderFilter filter)
        {
            var orders = DbContext.Orders.Include(o => o.Customer).Where(o => o.Producer == producer
                && o.ProducerId == producerId).ToList();

            await Filter(orders, filter);

            var vm = new OrderListVm();

            foreach (var order in orders)
            {
                vm.Orders.Add(Mapper.Map<OrderLookupVm>(order));
            }

            return vm;
        }

        public async Task Filter(List<Order> orders, OrderFilter filter)
        {
            if (filter == null) return;

            if(filter.Statuses != null)
            {
                orders = orders.Where(order => filter.Statuses.Contains(order.Status)).ToList();
            }
            if(filter.StartDate != default)
            {
                orders = orders.Where(o => o.CreationDate >= filter.StartDate).ToList();
            }
            if (filter.EndDate != default)
            {
                orders = orders.Where(o => o.CreationDate <= filter.EndDate).ToList();
            }
            if(filter.PaymentTypes != null && filter.PaymentTypes.Count > 0)
            {
                orders = orders.Where(o => filter.PaymentTypes.Contains(o.PaymentType)).ToList();
            }
        }

        public async Task<DashboardVm> GetDashboard(Guid id)
        {
            var monthStatistic = await DbContext.MonthesStatistics.FirstOrDefaultAsync(s => s.Id == id);

            if (monthStatistic == null)
                throw new NotFoundException($"Statistic for month with id {id} was not found.");

            var vm = Mapper.Map<DashboardVm>(monthStatistic);

            return vm;
        }

        public async Task<string> ExportToExcel(Guid producerId, Producer producer)
        {
            var orders = DbContext.Orders.Where(o => o.Producer == producer
                && o.ProducerId == producerId).ToList();

            string fileName = await GetFileName(producerId, producer);

            string filePath = Path.Combine(Configuration["Files"], fileName);
            using (FileStream fs = File.Create(filePath)) { }

            using (var fastExcel = new FastExcel.FastExcel(new FileInfo(filePath)))
            {
                var worksheet = new Worksheet();
                var rows = new List<Row>();
                var cells = new List<Cell>();

                cells.Add(new Cell(1, "Id"));
                cells.Add(new Cell(2, "Номер"));
                cells.Add(new Cell(3, "Дата замовлення"));
                cells.Add(new Cell(4, "Ім'я покупця"));
                cells.Add(new Cell(5, "Email покупця"));
                cells.Add(new Cell(6, "Телефон"));
                cells.Add(new Cell(7, "Сума"));
                cells.Add(new Cell(8, "Спосіб оплати"));
                cells.Add(new Cell(9, "Статус"));

                rows.Add(new Row(1, cells));

                for (int i = 0; i < orders.Count; i++)
                {
                    cells = new List<Cell>();
                    cells.Add(new Cell(1, orders[i].Id));
                    cells.Add(new Cell(2, orders[i].Number));
                    cells.Add(new Cell(3, orders[i].CreationDate));
                    cells.Add(new Cell(4, orders[i].Customer.Name + " " + orders[i].Customer.Surname));
                    cells.Add(new Cell(5, orders[i].Customer.Email));
                    cells.Add(new Cell(6, orders[i].Customer.Phone));
                    cells.Add(new Cell(7, orders[i].TotalPayment));

                    if(orders[i].PaymentType == PaymentType.Online) cells.Add(new Cell(8, "Онлайн"));
                    else if (orders[i].PaymentType == PaymentType.Cash) cells.Add(new Cell(8, "Готівка"));

                    if (orders[i].Status == OrderStatus.New) cells.Add(new Cell(9, "Нове"));
                    else if (orders[i].Status == OrderStatus.Processing) cells.Add(new Cell(9, "В обробці"));
                    else if (orders[i].Status == OrderStatus.Collected) cells.Add(new Cell(9, "Зібрано"));
                    else if (orders[i].Status == OrderStatus.InDelivery) cells.Add(new Cell(9, "В доставці"));
                    else if (orders[i].Status == OrderStatus.Completed) cells.Add(new Cell(9, "Виконано"));

                    rows.Add(new Row(i + 1, cells));
                }
                worksheet.Rows = rows;

                fastExcel.Update(worksheet, 1);
            }

            return fileName;
        }

        private async Task<string> GetFileName(Guid producerId, Producer producer)
        {
            string producerName = "";

            if (producer == Producer.Seller)
            {
                var account = await DbContext.Accounts.FirstOrDefaultAsync(a => a.Id == producerId
                    && a.Role == Role.Seller);

                producerName = account.Name + " " + account.Surname;
            }
            else if (producer == Producer.Farm)
            {
                var farm = await DbContext.Farms.FirstOrDefaultAsync(f => f.Id == producerId);
                producerName = farm.Name;
            }

            string fileName = producerName + "_" + DateTime.Now.ToString() + "_" + "orders.xlsx";

            return fileName;
        }

        public async Task<DashboardVm> GetCurrentMonthDashboard(Guid producerId, Producer producer)
        {
            var currentMonthStatistic = await StatisticService.CalculateStatisticForMonth(producerId, producer, DateTimeOffset.Now);

            var currentMonthDashboardVm = Mapper.Map<DashboardVm>(currentMonthStatistic);
            var customerWithHighestPayment = await DbContext.Accounts.FirstOrDefaultAsync(a => a.Id == currentMonthStatistic.CustomerWithHighestPaymentId);

            if (customerWithHighestPayment != null) throw new NotFoundException($"Customer with highest payment was not found(account id: {currentMonthStatistic.CustomerWithHighestPaymentId}).");

            currentMonthDashboardVm.CustomerWithHighestPaymentName = $"{customerWithHighestPayment.Surname} {customerWithHighestPayment.Name}";

            return currentMonthDashboardVm;
        }

        public async Task Duplicate(OrderListDto orderListDto)
        {
            foreach(var orderId in orderListDto.OrderIds)
            {
                var order = DbContext.Orders.FirstOrDefault(o => o.Id == orderId);

                if (order == null) 
                    throw new NotFoundException($"Order with id {orderId} was not found.");

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
                    DeliveryPointId = order.DeliveryPointId,
                    DeliveryPoint = order.DeliveryPoint,
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

        public async Task Delete(OrderListDto orderListDto)
        {
            foreach (var orderId in orderListDto.OrderIds)
            {
                var order = DbContext.Orders.FirstOrDefault(o => o.Id == orderId);

                if (order == null)
                    throw new NotFoundException($"Order with id {orderId} was not found.");

                foreach(var item in order.Items)
                {
                    DbContext.OrdersItems.Remove(item);
                }

                DbContext.Orders.Remove(order);
            }

            await DbContext.SaveChangesAsync();
        }

        public async Task<OrderVm> Get(Guid orderId)
        {
            var order = await DbContext.Orders.Include(o => o.Items).FirstOrDefaultAsync();

            if (order == null)
                throw new NotFoundException($"Order with id {orderId} was not found.");

            var vm = Mapper.Map<OrderVm>(order);
            var items = new List<OrderItemVm>();

            foreach (var item in order.Items)
            {
                var product = await DbContext.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);

                if (product == null)
                    throw new NotFoundException($"Product with id {item.ProductId} was not found.");

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

        public async Task Update(UpdateOrderDto updateOrderDto)
        {
            var order = await DbContext.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == updateOrderDto.OrderId);

            if (order == null)
                throw new NotFoundException($"Order with id {updateOrderDto.OrderId} was not found.");

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
                address.Longtitude = coords.Longitude;
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

        public async Task AddOrderItem(AddOrderItemDto addOrderItemDto)
        {
            var order = await DbContext.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == addOrderItemDto.OrderId);

            if (order == null)
                throw new NotFoundException($"Order with id {addOrderItemDto.OrderId} was not found.");

            var product = await DbContext.Products.FirstOrDefaultAsync(p => p.Id == addOrderItemDto.ProductId);

            if (product == null)
                throw new NotFoundException($"Product with id {addOrderItemDto.ProductId} was not found.");

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
