using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enum = HotelDirectory.Hotel.Service.Infrastructure.Data.Entities.Enum;

namespace HotelDirectory.Hotel.Service.Business.Model.Response
{
    public class DetailHotelResponse
    {
        public Guid HotelId { get; set; }
        public string CompanyName { get; set; }
        public string PersonName { get; set; }
        public string PersonSurname { get; set; }
        public List<DetailContactInfo> Contacts { get; set; }
    }

    public class DetailContactInfo
    {
        public Guid ContactId { get; set; }
        public Enum.ContactInfoType InfoType { get; set; }
        public string InfoContent { get; set; }
    }
}
