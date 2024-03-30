using AutoMapper;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Elasticsearch.Mappings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace FarmersMarketplace.Elasticsearch
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddElasticsearch(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(config =>
            {
                config.AddProfile(new ProductSearchProfile());
            });

            var url = configuration["ElasticsearchUrl"];

            var settings = new ConnectionSettings(new Uri(url)).PrettyJson();
            var client = new ElasticClient(settings);

            var configurator = new IndexConfigurator();
            configurator.Configure(client);

            services.AddSingleton<IElasticClient>(client);

            Task.Run(async () =>
            {
                using (var scope = services.BuildServiceProvider().CreateScope())
                {

                    var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
                    var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

                    await configurator.LoadData(client, dbContext, mapper);
                }
            }).Wait();

            return services;
        }
    }

}
