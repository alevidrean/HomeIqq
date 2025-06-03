using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public DbSet<AccessLog> AccessLog { get; set; }
        public DbSet<EventLog> EventLog { get; set; }
        public DbSet<TemperatureLog> TemperatureLog { get; set; }
        // public DbSet<TemperatureProgram> TemperatureProgram { get; set; }
        public DbSet<Device> Device { get; set; }


        public DbSet<TemperatureProgram> TemperaturePrograms { get; set; }
        public DbSet<TemperatureInterval> TemperatureIntervals { get; set; }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed roluri: Admin și User
            builder.Entity<Microsoft.AspNetCore.Identity.IdentityRole>().HasData(
                new Microsoft.AspNetCore.Identity.IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new Microsoft.AspNetCore.Identity.IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                }
            );
        }

    }
}