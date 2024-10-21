using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelDirectory.Reporting.Service.Infrastructure.RabbitMQClient.Interface;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace HotelDirectory.Reporting.Service.Infrastructure.RabbitMQClient.Base
{
    public partial class QueueOperation : IQueueOperation
    {
        private readonly IConnection _connection;

        public QueueOperation(IConnection connection)
        {
            _connection = connection;
        }
        public void ReportMessage<T>(T message)
        {
            using (var channel = _connection.CreateModel())
            {
                channel.QueueDeclare(queue: "reports", durable: false, exclusive: false, autoDelete: false, arguments: null);

                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange: "", routingKey: "reports", basicProperties: null, body: body);

            }
        }
    }
}
