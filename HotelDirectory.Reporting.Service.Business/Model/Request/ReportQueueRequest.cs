using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelDirectory.Reporting.Service.Business.Model.Request
{
    public class ReportQueueRequest
    {
        public Guid ReportId { get; set; }
        public string Location { get; set; }
    }
}
