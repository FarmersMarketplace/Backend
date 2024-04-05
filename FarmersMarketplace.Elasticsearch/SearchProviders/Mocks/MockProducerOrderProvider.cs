using FarmersMarketplace.Application.DataTransferObjects.Order;
using FarmersMarketplace.Application.Filters;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Application.ViewModels.Order;
using FarmersMarketplace.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FarmersMarketplace.Elasticsearch.SearchProviders.Mocks
{
    public class MockProducerOrderProvider : ISearchProvider<GetProducerOrderListDto, ProducerOrderListVm, ProducerOrderAutocompleteDto>
    {
        private readonly IApplicationDbContext DbContext;

        public MockProducerOrderProvider(IApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<List<string>> Autocomplete(ProducerOrderAutocompleteDto request)
        {
            return await DbContext.Products.Where(p => p
                    .Name.Contains(request.Query)
                    && p.Producer == request.Producer
                    && p.ProducerId == request.ProducerId)
                .Take(request.Count)
                .Select(p => p.Name)
                .ToListAsync();
        }

        public async Task<ProducerOrderListVm> Search(GetProducerOrderListDto request)
        {
            var orders = DbContext.Orders.Include(o => o.Customer)
                .Where(o => o.Producer == request.Producer
                    && o.ProducerId == request.ProducerId)
                .ToList();

            if (request.Filter != null)
            {
                orders = await ApplyFilter(request.Filter, orders);
            }

            if (!request.Query.IsNullOrEmpty())
            {
                orders = orders.Where(p => p.Number.ToString("D7")
                        .Contains(request.Query))
                    .ToList();
            }

            orders = orders.Skip((request.Page - 1) * request.PageSize)
               .Take(request.PageSize)
               .ToList();

            var vm = new ProducerOrderListVm
            {
                Orders = new List<ProducerOrderLookupVm>(),
                Count = orders.Count
            };

            for (int i = 0; i < orders.Count; i++)
            {
                vm.Orders.Add(new ProducerOrderLookupVm
                {
                    Id = orders[i].Id,
                    Number = orders[i].Number.ToString("D7"),
                    CustomerName = orders[i].CustomerName,
                    CustomerPhone = orders[i].CustomerPhone,
                    CustomerEmail = orders[i].Customer.Email,
                    TotalPayment = orders[i].TotalPayment,
                    PaymentType = orders[i].PaymentType,
                    Status = orders[i].Status,
                });
            }

            return vm;
        }

        private async Task<List<Order>> ApplyFilter(ProducerOrderFilter filter, List<Order> orders)
        {
            if (filter.Statuses != null && filter.Statuses.Any())
            {
                orders = orders.Where(p => filter.Statuses.Contains(p.Status)).ToList();
            }

            if (filter.PaymentTypes != null && filter.PaymentTypes.Any())
            {
                orders = orders.Where(p => filter.PaymentTypes.Contains(p.PaymentType)).ToList();
            }

            if (filter.MaximumAmount.HasValue)
            {
                orders = orders.Where(p => p.TotalPayment <= filter.MaximumAmount).ToList();
            }

            if (filter.MinimumAmount.HasValue)
            {
                orders = orders.Where(p => p.TotalPayment >= filter.MinimumAmount).ToList();
            }

            if (filter.StartDate.HasValue)
            {
                orders = orders.Where(p => p.CreationDate >= filter.StartDate).ToList();
            }

            if (filter.EndDate.HasValue)
            {
                orders = orders.Where(p => p.CreationDate <= filter.EndDate).ToList();
            }

            return orders;
        }
    }

}
