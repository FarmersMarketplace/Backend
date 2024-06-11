using AutoMapper;
using FarmersMarketplace.Application.DataTransferObjects.Order;
using FarmersMarketplace.Application.DataTransferObjects.Producers;
using FarmersMarketplace.Application.DataTransferObjects.Product;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Application.ViewModels.Order;
using FarmersMarketplace.Application.ViewModels.Producers;
using FarmersMarketplace.Application.ViewModels.Product;
using FarmersMarketplace.Elasticsearch.Documents;
using FarmersMarketplace.Elasticsearch.Mappings;
using FarmersMarketplace.Elasticsearch.SearchProviders;
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

            services.AddTransient
               <ISearchProvider<GetCustomerProductListDto, CustomerProductListVm, CustomerProductAutocompleteDto>,
               CustomerProductSearchProvider>();
            services.AddTransient
                <ISearchProvider<GetProducerProductListDto, ProducerProductListVm, ProducerProductAutocompleteDto>,
                ProducerProductSearchProvider>();
            services.AddTransient
               <ISearchProvider<GetCustomerOrderListDto, CustomerOrderListVm, CustomerOrderAutocompleteDto>,
               CustomerOrderSearchProvider>();
            services.AddTransient
                <ISearchProvider<GetProducerOrderListDto, ProducerOrderListVm, ProducerOrderAutocompleteDto>,
                ProducerOrderSearchProvider>();
            services.AddTransient
                <ISearchProvider<GetProducerListDto, ProducerListVm, ProducerAutocompleteDto>,
                ProducerSearchProvider>();

            var settings = new ConnectionSettings(new Uri(configuration["ElasticsearchUrl"]))
                .EnableApiVersioningHeader()
                .PrettyJson();
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

                    //await configurator.LoadData(client, dbContext, mapper);
                }
            }).Wait();

            return services;
        }
    }

}
