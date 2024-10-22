using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelDirectory.Hotel.Service.Business.Model.Response
{
    public class HotelInfoResponse
    {
        public Guid HotelId { get; set; }
        public string CompanyName { get; set; }
        public string PersonName { get; set; }
        public string PersonSurname { get; set; }

    }
}
