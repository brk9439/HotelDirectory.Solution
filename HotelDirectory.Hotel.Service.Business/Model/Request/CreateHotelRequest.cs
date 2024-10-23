using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelDirectory.Hotel.Service.Business.Model.Request
{
    public class CreateHotelRequest
    {
        public string PersonName { get; set; }
        public string PersonSurname { get; set; }
        public string CompanyName { get; set; }
        
    }
}
