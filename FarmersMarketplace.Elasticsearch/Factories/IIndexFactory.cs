using AutoMapper;
using FarmersMarketplace.Application.Interfaces;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Elasticsearch.Factories
{
    public interface IIndexFactory
    {
        void CreateIndex(IElasticClient client);
        CreateIndexDescriptor ConfigureIndex(CreateIndexDescriptor descriptor);
        Task LoadData(IElasticClient client, IApplicationDbContext dbContext, IMapper mapper);
    }
}
