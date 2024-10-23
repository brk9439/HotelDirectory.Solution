namespace HotelDirectory.Reporting.Service.Consumer.Configuration
{
    public class ConfigManager
    {
        private IConfiguration _configuration { get; }

        public ConfigManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ConsumeQueueConfiguration ConsumeQueueConfiguration => new ConsumeQueueConfiguration
        {
            QueueName = _configuration.GetValue<string>("ConsumeQueueConfiguration:QueueName"),
            ExchangeName = _configuration.GetValue<string>("ConsumeQueueConfiguration:ExchangeName"),
            ExchangeType = _configuration.GetValue<string>("ConsumeQueueConfiguration:ExchangeType"),
            RoutingKeyName = _configuration.GetValue<string>("ConsumeQueueConfiguration:RoutingKeyName"),
            PrefetchCount = _configuration.GetValue<ushort>("ConsumeQueueConfiguration:PrefetchCount")
        };

    }

    public class ConsumeQueueConfiguration
    {
        public string QueueName { get; set; }
        public string ExchangeName { get; set; }
        public string ExchangeType { get; set; }
        public string RoutingKeyName { get; set; }
        public ushort PrefetchCount { get; set; }
    }

}
