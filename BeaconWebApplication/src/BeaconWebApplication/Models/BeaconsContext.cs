using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;

namespace BeaconWebApplication.Models
{
    public class BeaconsContext : DbContext
    {
        public BeaconsContext()
        {
            Database.EnsureCreated();
        }
        public DbSet<Beacon> Beacons { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connString = Startup.Configuration["Data:BeaconsContextConnection:ConnectionString"];
            optionsBuilder.UseSqlServer(connString);

            base.OnConfiguring(optionsBuilder);
        }
    }
}
