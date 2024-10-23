using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelDirectory.Hotel.Service.Business.Configuration;
using HotelDirectory.Hotel.Service.Business.Business;

namespace HotelDirectory.Hotel.Service.Business.Extension
{
    public static class BusinessExtension
    {

        public static IServiceCollection RegisterService(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddSingleton<ConfigManager>();
            services.AddScoped<IOperationBusiness, OperationBusiness>();
            HotelDirectory.Hotel.Service.Infrastructure.Extension.InfrastructureExtension.RegisterService(services, configuration);
            HotelDirectory.Shared.ElasticSearch.Extension.ElasticSearchExtension.RegisterService(services, configuration);
            return services;
        }
    }
}
