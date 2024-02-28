using AllPhi.HoGent.Datalake.Data.Context;
using AllPhi.HoGent.Datalake.Data.Helpers;
using AllPhi.HoGent.Datalake.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AllPhi.HoGent.Datalake.Data.Store
{
    public class VehicleStore : IVehicleStore
    {
        private readonly IDbContextFactory<AllPhiDatalakeContext> _dbContextFactory;

        public VehicleStore(IDbContextFactory<AllPhiDatalakeContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<Vehicle> GetVehicleByIdAsync(Guid vehicleId)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            return await dbContext.Vehicles.FirstOrDefaultAsync(x => x.Id == vehicleId);
        }

        public async Task<(List<Vehicle>, int)> GetAllVehiclesAsync([Optional] string? sortBy, [Optional] bool isAscending, Pagination? pagination = null)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            List<Vehicle> vehicles = new();
            IQueryable<Vehicle> vehiclesQuery = dbContext.Vehicles;

            IQueryable<Vehicle> sortedVehicles = sortBy switch
            {
                "id" => isAscending ? vehiclesQuery.OrderBy(x => x.Id) : vehiclesQuery.OrderByDescending(x => x.Id),
                "chassisnumber" => isAscending ? vehiclesQuery.OrderBy(x => x.ChassisNumber) : vehiclesQuery.OrderByDescending(x => x.ChassisNumber),
                "color" => isAscending ? vehiclesQuery.OrderBy(x => x.VehicleColor) : vehiclesQuery.OrderByDescending(x => x.VehicleColor),
                "carband" => isAscending ? vehiclesQuery.OrderBy(x => x.CarBrand) : vehiclesQuery.OrderByDescending(x => x.CarBrand),
                "licenseplate" => isAscending ? vehiclesQuery.OrderBy(x => x.LicensePlate) : vehiclesQuery.OrderByDescending(x => x.LicensePlate),
                _ => vehiclesQuery
            };

            var totalItems = await sortedVehicles.CountAsync();
            if (pagination != null)
            {
                sortedVehicles = sortedVehicles.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize);
            }

            vehicles = await sortedVehicles.ToListAsync();

            return (vehicles, totalItems);
        }

        public async Task AddVehicle(Vehicle vehicle)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            bool existingvehicle = await dbContext.Vehicles.AnyAsync(x => x.ChassisNumber == vehicle.ChassisNumber);

            if (!existingvehicle)
            {
                try
                {
                    await dbContext.Database.BeginTransactionAsync();

                    await dbContext.Vehicles.AddAsync(vehicle);

                    await dbContext.SaveChangesAsync();

                    await dbContext.Database.CommitTransactionAsync();
                }
                catch
                {
                    await dbContext.Database.RollbackTransactionAsync();
                    throw;
                }

            }
            else
                throw new Exception("Vehicle already exist.");
        }

        public async Task UpdateVehicle(Vehicle vehicle)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            var existingVehicle = await dbContext.Vehicles.FindAsync(vehicle.Id);
            if (existingVehicle == null)
            {
                throw new Exception("Vehicle not found.");
            }

            try
            {
                await dbContext.Database.BeginTransactionAsync();

                dbContext.Entry(existingVehicle).CurrentValues.SetValues(vehicle);

                await dbContext.SaveChangesAsync();

                await dbContext.Database.CommitTransactionAsync();
            }
            catch
            {
                await dbContext.Database.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task RemoveVehicle(Guid vehicleId)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            var existingVehicle = await dbContext.Vehicles.FindAsync(vehicleId);
            if (existingVehicle == null)
            {
                throw new Exception("Vehicle not found.");
            }

            try
            {
                await dbContext.Database.BeginTransactionAsync();

                dbContext.Vehicles.Remove(existingVehicle);

                await dbContext.SaveChangesAsync();

                await dbContext.Database.CommitTransactionAsync();
            }
            catch
            {
                await dbContext.Database.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
