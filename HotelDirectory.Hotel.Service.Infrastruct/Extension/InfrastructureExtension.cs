using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelDirectory.Hotel.Service.Infrastructure.Data.Context;

namespace HotelDirectory.Hotel.Service.Infrastructure.Extension
{
    public static class InfrastructureExtension
    {
        public static IServiceCollection RegisterService(IServiceCollection services, IConfigurationRoot configuration)
        {
            #region Postgres
            

            
            #endregion


            return services;
        }
    }
}
