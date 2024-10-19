using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelDirectory.Hotel.Service.Infrastruct.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelDirectory.Hotel.Service.Infrastruct.Data.Context
{
    public class HotelDbContext:DbContext
    {

        public DbSet<HotelsInfo> HotelsInfo { get; set; }
        public DbSet<ContactInfo> ContactInfo { get; set; }
    }
}
