using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelDirectory.Hotel.Service.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelDirectory.Hotel.Service.Infrastructure.Data.Context
{
    public class HotelDbContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                "Server=localhost;Port=5435;Database=hoteldb;User Id=hotel_user;Password=hotel_password");
        }
        public DbSet<HotelInfo> HotelInfo { get; set; }
        public DbSet<ContactInfo> ContactInfo { get; set; }

        public DbSet<ReportingInfo> ReportingInfo { get; set; }
    }
}
