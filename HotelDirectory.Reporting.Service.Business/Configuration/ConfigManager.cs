using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace HotelDirectory.Reporting.Service.Business.Configuration
{
    public class ConfigManager
    {
        private IConfiguration _configuration { get; }

        public ConfigManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ReportQueueMessageConfiguration ReportQueueMessageConfiguration => new ReportQueueMessageConfiguration
        {
            QueueName = _configuration.GetValue<string>("ReportQueueMessageConfiguration:QueueName"),
            ExchangeName = _configuration.GetValue<string>("ReportQueueMessageConfiguration:ExchangeName"),
            RoutingKeyName = _configuration.GetValue<string>("ReportQueueMessageConfiguration:RoutingKeyName"),
            MessageTtl = _configuration.GetValue<int>("ReportQueueMessageConfiguration:MessageTtl")
        };
        
    }
    public class ReportQueueMessageConfiguration
    {
        public string QueueName { get; set; }
        public string ExchangeName { get; set; }
        public string RoutingKeyName { get; set; }
        public int MessageTtl { get; set; }
    }
}
