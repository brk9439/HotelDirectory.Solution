﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelDirectory.Hotel.Service.Infrastructure.Data.Entities
{
    public class Enum
    {
        public enum ContactInfoType
        {
            PhoneNumber = 1,
            MailAddress = 2,
            Location = 3,
        }

        public enum ReportStatus
        {
            Waiting = 0,
            Completed = 1,
        }

        public enum Status
        {
            Passive = 0,
            Active = 1
        }
    }
}
