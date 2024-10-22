using HotelDirectory.Reporting.Service.Consumer.Consumer;
using HotelDirectory.Reporting.Service.Infrastructure.RabbitMQClient.Interface;

namespace HotelDirectory.Reporting.Service.Consumer.Extension
{
    public static class ConsumerExtension
    {
        public static IServiceCollection RegisterService(IServiceCollection services, IConfigurationRoot configuration)
        {
            //services.AddSingleton<ConfigManager>();
            services.AddHostedService<ReportConsumer>();
            HotelDirectory.Reporting.Service.Infrastructure.Extension.InfrastructureExtension.RegisterService(services, configuration);
            ConfigureReportQueueSystem(services, configuration);
            return services;
        }

        public static void ConfigureReportQueueSystem(IServiceCollection services, IConfigurationRoot configuration)
        {
            var serviceBuilder = services.BuildServiceProvider();
            var queueService = serviceBuilder.GetService<IQueueOperation>();

            Dictionary<string, object> exchangeArgs = new Dictionary<string, object>
            {
                { "x-message-ttl", 0 }
            };
            bool queueResult = false;
            bool exchangeResult = false;

            exchangeResult = queueService.ConfigureExchange("report_direct", "direct", true, false, exchangeArgs);
            if (exchangeResult)
            {
                queueResult = queueService.ConfigureQueue("report_queue", true, false, false, null);
            }

            if (queueResult)
            {
                bool bindResult = queueService.BindQueueToExchange("report_queue", "report_direct", "report_key", null);
            }
        }
    }
}
