using FarmersMarketplace.Elasticsearch.Factories;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            };
        }

        public void Configure(IElasticClient client)
        {
            foreach (var factory in IndexFactories)
            {
                factory.CreateIndex(client);
            }
        }
    }

}
