using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelDirectory.Reporting.Service.Business.Model.Response
{
    public class ReportResponse
    {
        public string Location { get; set; }
        public int HotelCount { get; set; }
        public int PhoneCount { get; set; }
        public DateTime ReportStartDate { get; set; }
        public DateTime? ReportEndDate { get; set; }
        public string Status { get; set; }
    }
}
