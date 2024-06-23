using AutoMapper;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Application.ViewModels.Producers;
using FarmersMarketplace.Domain;
using Microsoft.EntityFrameworkCore;

namespace FarmersMarketplace.Application.Services.Business
{
    public class FarmTrendService
    {
        private readonly int PopularEntitiesCount;
        private readonly IApplicationDbContext DbContext;
        private readonly IMapper Mapper;
        private List<ProducerLookupVm> PopularFarms { get; set; }
        private HashSet<Guid> FarmsIds { get; set; }
        private DateTime UpdateDate { get; set; }
        private readonly int UpdateTime;

        public FarmTrendService(IApplicationDbContext dbContext, IMapper mapper, int popularEntitiesCount, int updateTime)
        {
            DbContext = dbContext;
            PopularEntitiesCount = popularEntitiesCount;
            UpdateTime = updateTime;
            UpdateDate = default;
            Mapper = mapper;
        }

        public async Task<ProducerListVm> UpdateAndGet()
        {
            if (DateTime.Now - UpdateDate >= TimeSpan.FromMinutes(UpdateTime))
            {
                var popularFarms = await DbContext.Farms
                    .Select(f => new
                    {
                        Farm = f,
                        OrderCount = DbContext.Orders.Count(o => o.ProducerId == f.Id && o.Producer == Producer.Farm)
                    })
                    .OrderByDescending(f => f.OrderCount)
                    .Take(PopularEntitiesCount)
                    .OrderByDescending(f => f.Farm.Feedbacks.AverageRating)
                    .Select(f => Mapper.Map<ProducerLookupVm>(f.Farm))
                    .ToListAsync();

                FarmsIds = popularFarms.Select(p => p.Id).ToHashSet();
                UpdateDate = DateTime.Now;

            }

            var vm = new ProducerListVm
            {
                Producers = PopularFarms,
            };

            return vm;
        }

        public async Task UpdateIfPopular(Farm farm)
        {
            if (FarmsIds.Contains(farm.Id))
            {
                await UpdateAndGet();
            }
        }
    }
}
