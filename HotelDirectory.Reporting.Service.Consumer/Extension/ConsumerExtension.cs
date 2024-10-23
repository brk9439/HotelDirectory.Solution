using HotelDirectory.Reporting.Service.Consumer.Configuration;
using HotelDirectory.Reporting.Service.Consumer.Consumer;
using HotelDirectory.Reporting.Service.Infrastructure.RabbitMQClient.Interface;

namespace HotelDirectory.Reporting.Service.Consumer.Extension
{
    public static class ConsumerExtension
    {
        public static IServiceCollection RegisterService(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddSingleton<ConfigManager>();
            services.AddHostedService<ReportConsumer>();
            HotelDirectory.Reporting.Service.Infrastructure.Extension.InfrastructureExtension.RegisterService(services, configuration);
            HotelDirectory.Shared.ElasticSearch.Extension.ElasticSearchExtension.RegisterService(services, configuration);
            ConfigureReportQueueSystem(services, configuration);
            return services;
        }

        public static void ConfigureReportQueueSystem(IServiceCollection services, IConfigurationRoot configuration)
        {
            var serviceBuilder = services.BuildServiceProvider();
            var queueService = serviceBuilder.GetService<IQueueOperation>();

            Dictionary<string, object> exchangeArgs = new Dictionary<string, object>
            {
                { "x-message-ttl", configuration.GetValue<int>("QueueOperationConfigureExchange:MessageTtl") }
            };
            bool queueResult = false;
            bool exchangeResult = false;

            exchangeResult = queueService.ConfigureExchange(
                configuration.GetValue<string>("QueueOperationConfigureExchange:ExchangeName"),
                configuration.GetValue<string>("QueueOperationConfigureExchange:Type"),
                configuration.GetValue<bool>("QueueOperationConfigureExchange:Durable"),
                configuration.GetValue<bool>("QueueOperationConfigureExchange:AutoDelete"),
                exchangeArgs);

            if (exchangeResult)
            {
                queueResult = queueService.ConfigureQueue(
                    configuration.GetValue<string>("QueueOperationConfigureQueue:QueueName"),
                    configuration.GetValue<bool>("QueueOperationConfigureQueue:Durable"),
                    configuration.GetValue<bool>("QueueOperationConfigureQueue:Exclusive"),
                    configuration.GetValue<bool>("QueueOperationConfigureQueue:AutoDelete"),
                    null);
            }

            if (queueResult)
            {
                bool bindResult = queueService.BindQueueToExchange(
                    configuration.GetValue<string>("QueueOperationBindQueueToExchange:QueueName"),
                    configuration.GetValue<string>("QueueOperationBindQueueToExchange:ExchangeName"),
                    configuration.GetValue<string>("QueueOperationBindQueueToExchange:RoutingKeyName"),
                    null);
            }
        }
    }
}
