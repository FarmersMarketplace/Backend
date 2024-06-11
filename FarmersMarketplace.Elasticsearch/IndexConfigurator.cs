using AutoMapper;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Elasticsearch.Factories;
using Nest;

namespace FarmersMarketplace.Elasticsearch
{
    public class IndexConfigurator
    {
        private List<IIndexFactory> IndexFactories;

        public IndexConfigurator()
        {
            IndexFactories = new List<IIndexFactory> 
            {
                new ProductIndexFactory(),
                new OrderIndexFactory(),
                new ProducerIndexFactory(),
            };
        }

        public void Configure(IElasticClient client)
        {
            foreach (var factory in IndexFactories)
            {
                factory.CreateIndex(client);
            }
        }

        public async Task LoadData(IElasticClient client, IApplicationDbContext dbContext, IMapper mapper)
        {
            var tasks = new Task[IndexFactories.Count];

            for (int i = 0; i < IndexFactories.Count; i++)
            {
                tasks[i] = IndexFactories[i].LoadData(client, dbContext, mapper);
            }

            await Task.WhenAll(tasks);
        }
        
    }

}
