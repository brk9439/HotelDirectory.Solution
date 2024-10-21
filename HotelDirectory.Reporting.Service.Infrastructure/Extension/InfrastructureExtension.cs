using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelDirectory.Reporting.Service.Infrastructure.Data.Context;
using HotelDirectory.Reporting.Service.Infrastructure.RabbitMQClient.Base;
using HotelDirectory.Reporting.Service.Infrastructure.RabbitMQClient.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace HotelDirectory.Reporting.Service.Infrastructure.Extension
{
    public static class InfrastructureExtension
    {
        public static IServiceCollection RegisterService(IServiceCollection services, IConfigurationRoot configuration)
        {
            #region Postgres
            //PostgreSql Timestamp
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            services.AddDbContext<HotelDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetSection("ConnectionStrings:HotelDbConnection").Value);
            });
            #endregion

            #region RabbitMQ

            var connectionFactory = new ConnectionFactory()
            {
                HostName = configuration.GetSection("RabbitMQConfiguration").GetValue<string>("Host"),
                Port = Convert.ToInt32(configuration.GetSection("RabbitMQConfiguration").GetValue<string>("Port")),
                UserName = configuration.GetSection("RabbitMQConfiguration").GetValue<string>("Username"),
                Password = configuration.GetSection("RabbitMQConfiguration").GetValue<string>("Password")

            };
            // Otomatik bağlantı kurtarmayı etkinleştirmek için,
            connectionFactory.AutomaticRecoveryEnabled = true;
            // Her 10 sn de bir tekrar bağlantı toparlanmaya çalışır 
            connectionFactory.NetworkRecoveryInterval = TimeSpan.FromSeconds(10);
            // sunucudan bağlantısı kesildikten sonra kuyruktaki mesaj tüketimini sürdürmez 
            // (TopologyRecoveryEnabled = false olarak tanımlandığı için)
            connectionFactory.TopologyRecoveryEnabled = false;
            IConnection? rabbitConnection = connectionFactory.CreateConnection();
            services.AddSingleton(rabbitConnection);

            services.AddSingleton<IQueueOperation, QueueOperation>();

            #endregion
            return services;
        }
    }
}
