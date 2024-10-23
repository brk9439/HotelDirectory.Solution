using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HotelDirectory.Hotel.Service.Infrastructure.Data.Entities.Enum;

namespace HotelDirectory.Hotel.Service.Infrastructure.Data.Entities
{
    public class HotelInfo
    {
        public Guid Id { get; set; }
        public string PersonName { get; set; }
        public string PersonSurname { get; set; }
        public string CompanyName { get; set; }
        public Status Status { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public HotelInfo()
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.UtcNow;
        }
    }
}
