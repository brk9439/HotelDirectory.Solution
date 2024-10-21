using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HotelDirectory.Hotel.Service.Infrastructure.Data.Entities.Enum;

namespace HotelDirectory.Hotel.Service.Business.Model.Request
{
    public class CreateContactRequest
    {
        public Guid HotelId { get; set; }
        public ContactInfoType InfoType { get; set; }
        public string InfoContent { get; set; }
    }
}
