using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelDirectory.Reporting.Service.Infrastructure.Data.Entities
{
    public class ContactInfo
    {
        public Guid Id { get; set; }
        public Guid FK_HotelInfo { get; set; }
        public Enum.ContactInfoType InfoType { get; set; }
        public string InfoContent { get; set; }
        public Enum.Status Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public ContactInfo()
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.Now;
        }
    }
}
