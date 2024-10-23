using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelDirectory.Reporting.Service.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HotelDirectory.Reporting.Service.Infrastructure.Data.Context
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ReportingInfo>(entity =>
            {
                entity.ToTable("ReportingInfo");
            });
            builder.Entity<ContactInfo>(entity =>
            {
                entity.ToTable("ContactInfo");
            });

            base.OnModelCreating(builder);
        }

        public DbSet<ReportingInfo> ReportingInfo { get; set; }
        public DbSet<ContactInfo> ContactInfo { get; set; }
    }
}
