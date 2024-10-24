﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelDirectory.Reporting.Service.Business.Business;
using HotelDirectory.Reporting.Service.Business.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelDirectory.Reporting.Service.Business.Extension
{
    public static class BusinessExtension
    {
        public static IServiceCollection RegisterService(IServiceCollection services, IConfigurationRoot configuration)
        {

            services.AddSingleton<ConfigManager>();
            HotelDirectory.Reporting.Service.Infrastructure.Extension.InfrastructureExtension.RegisterService(services, configuration);
            HotelDirectory.Shared.ElasticSearch.Extension.ElasticSearchExtension.RegisterService(services, configuration);
            services.AddScoped<IReportOperationBusiness, ReportOperationBusiness>();

            
            return services;
        }
    }
}
