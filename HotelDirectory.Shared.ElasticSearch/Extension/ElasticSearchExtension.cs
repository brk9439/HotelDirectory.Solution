using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelDirectory.Shared.ElasticSearch.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace HotelDirectory.Shared.ElasticSearch.Extension
{
    public static class ElasticSearchExtension
    {
        public static IServiceCollection RegisterService(IServiceCollection services, IConfigurationRoot configuration)
        {

            #region ElasticSearch
            var elasticSearchUrl = configuration["ConnectionStrings:ElasticSearchConnection"];
            var indexName = "elastic-log";
            var settings = new ConnectionSettings(new Uri(elasticSearchUrl)).DefaultIndex(indexName);
            var elasticClient = new ElasticClient(settings);
            services.AddSingleton<IElasticClient>(elasticClient);
            //services.AddScoped<IElasticSearchLogger<GenericLogModel>, ElasticSearchLogger<GenericLogModel>>();
            services.AddScoped(typeof(IElasticSearchLogger<>), typeof(ElasticSearchLogger<>));
            #endregion

            return services;
        }
    }
}
