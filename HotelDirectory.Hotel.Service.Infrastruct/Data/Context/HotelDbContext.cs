﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using HotelDirectory.Hotel.Service.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HotelDirectory.Hotel.Service.Infrastructure.Data.Context
{
    public class HotelDbContext : DbContext
    {
       public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<HotelInfo>(entity =>
            {
                entity.ToTable("HotelInfo");
            });
            builder.Entity<ContactInfo>(entity =>
            {
                entity.ToTable("ContactInfo");
            });

            base.OnModelCreating(builder);

        }
        public DbSet<HotelInfo> HotelInfo { get; set; }
        public DbSet<ContactInfo> ContactInfo { get; set; }

    }
}
