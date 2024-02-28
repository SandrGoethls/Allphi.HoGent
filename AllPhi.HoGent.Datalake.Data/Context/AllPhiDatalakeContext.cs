using AllPhi.HoGent.Datalake.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AllPhi.HoGent.Datalake.Data.Context
{
    public class AllPhiDatalakeContext : DbContext
    {
        public AllPhiDatalakeContext(DbContextOptions<AllPhiDatalakeContext> options) : base(options)
        {

        }

        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<FuelCard> FuelCards { get; set; }
        public DbSet<FuelCardFuelType> FuelCardFuelTypes { get; set; }
        public DbSet<FuelCardDriver> FuelCardDrivers { get; set; }
        public DbSet<DriverVehicle> DriverVehicle { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FuelCardDriver>().HasKey(fcd => new { fcd.DriverId, fcd.FuelCardId });
            modelBuilder.Entity<DriverVehicle>().HasKey(dv => new { dv.DriverId, dv.VehicleId });

            modelBuilder.Entity<Vehicle>().Property(e => e.NumberOfDoors).HasConversion<string>();
            modelBuilder.Entity<Vehicle>().Property(e => e.TypeOfCar).HasConversion<string>();
            modelBuilder.Entity<Vehicle>().Property(e => e.VehicleColor).HasConversion<string>();
            modelBuilder.Entity<Vehicle>().Property(e => e.FuelType).HasConversion<string>();
            modelBuilder.Entity<Vehicle>().Property(e => e.CarBrand).HasConversion<string>();

            modelBuilder.Entity<Driver>().Property(e => e.TypeOfDriverLicense).HasConversion<string>();

            modelBuilder.Entity<FuelCardFuelType>().Property(e => e.FuelType).HasConversion<string>();
        }
    }
}
