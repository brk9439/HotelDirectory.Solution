using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelDirectory.Reporting.Service.Infrastructure.RabbitMQClient.Interface
{
    public interface IQueueOperation
    {
        void ReportMessage<T>(T message);
    }
}
