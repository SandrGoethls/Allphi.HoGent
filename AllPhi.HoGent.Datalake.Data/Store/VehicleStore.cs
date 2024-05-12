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
        private readonly AllPhiDatalakeContext _dbContext;
        public VehicleStore(AllPhiDatalakeContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Vehicle> GetVehicleByIdAsync(Guid vehicleId)
        {
            return await _dbContext.Vehicles.FirstOrDefaultAsync(x => x.Id == vehicleId) ?? new();
        }

        public async Task<(List<Vehicle>, int)> GetAllVehiclesAsync(FilterVehicle? filterVehicle, [Optional] string? sortBy, [Optional] bool isAscending, Pagination? pagination = null)
        {
            List<Vehicle> vehicles = new();
            IQueryable<Vehicle> vehiclesQuery = _dbContext.Vehicles;         

            IQueryable<Vehicle> sortedVehicles = sortBy switch
            {
                "id" => isAscending ? vehiclesQuery.OrderBy(x => x.Id) : vehiclesQuery.OrderByDescending(x => x.Id),
                "chassisnumber" => isAscending ? vehiclesQuery.OrderBy(x => x.ChassisNumber) : vehiclesQuery.OrderByDescending(x => x.ChassisNumber),
                "licenseplate" => isAscending ? vehiclesQuery.OrderBy(x => x.LicensePlate) : vehiclesQuery.OrderByDescending(x => x.LicensePlate),
                "carbrand" => isAscending ? vehiclesQuery.OrderBy(x => x.CarBrand) : vehiclesQuery.OrderByDescending(x => x.CarBrand),
                "fueltype" => isAscending ? vehiclesQuery.OrderBy(x => x.FuelType) : vehiclesQuery.OrderByDescending(x => x.FuelType),
                "typeofcar" => isAscending ? vehiclesQuery.OrderBy(x => x.TypeOfCar) : vehiclesQuery.OrderByDescending(x => x.TypeOfCar),
                "color" => isAscending ? vehiclesQuery.OrderBy(x => x.VehicleColor) : vehiclesQuery.OrderByDescending(x => x.VehicleColor),
                "numberofdoors" => isAscending ? vehiclesQuery.OrderBy(x => x.NumberOfDoors) : vehiclesQuery.OrderByDescending(x => x.NumberOfDoors),
                "status" => isAscending ? vehiclesQuery.OrderBy(x => x.Status) : vehiclesQuery.OrderByDescending(x => x.Status),
                _ => vehiclesQuery
            };

            if (filterVehicle != null)
            {
                if (!string.IsNullOrEmpty(filterVehicle.SearchByLicencePlate))
                    sortedVehicles = sortedVehicles.Where(x => x.LicensePlate.Contains(filterVehicle.SearchByLicencePlate));

                if (!string.IsNullOrEmpty(filterVehicle.SearchByChassisNumber))
                    sortedVehicles = sortedVehicles.Where(x => x.ChassisNumber.Contains(filterVehicle.SearchByChassisNumber));
            }

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
            bool existingvehicle = VehicleWithChassisNumberExists(vehicle.ChassisNumber);
            bool existingLicensePlate = VehicleWithLicensePlateExists(vehicle.LicensePlate);

            if (!existingvehicle && !existingLicensePlate)
            {
                try
                {
                    await _dbContext.Database.BeginTransactionAsync();

                    await _dbContext.Vehicles.AddAsync(vehicle);

                    await _dbContext.SaveChangesAsync();

                    await _dbContext.Database.CommitTransactionAsync();
                }
                catch
                {
                    await _dbContext.Database.RollbackTransactionAsync();
                    throw;
                }

            }
            else
                throw new Exception("Vehicle already exist.");
        }

        public async Task UpdateVehicle(Vehicle vehicle)
        {
            var existingVehicle = await _dbContext.Vehicles.FindAsync(vehicle.Id);
            if (existingVehicle == null)
            {
                throw new Exception("Vehicle not found.");
            }

            try
            {
                await _dbContext.Database.BeginTransactionAsync();

                _dbContext.Entry(existingVehicle).CurrentValues.SetValues(vehicle);

                await _dbContext.SaveChangesAsync();

                await _dbContext.Database.CommitTransactionAsync();
            }
            catch
            {
                await _dbContext.Database.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task RemoveVehicle(Guid vehicleId)
        {
            var existingVehicle = await _dbContext.Vehicles.FindAsync(vehicleId);
            if (existingVehicle == null)
            {
                throw new Exception("Vehicle not found.");
            }

            try
            {
                await _dbContext.Database.BeginTransactionAsync();

                _dbContext.Vehicles.Remove(existingVehicle);

                await _dbContext.SaveChangesAsync();

                await _dbContext.Database.CommitTransactionAsync();
            }
            catch
            {
                await _dbContext.Database.RollbackTransactionAsync();
                throw;
            }
        }

        public bool VehicleWithChassisNumberExists(string chassisNumber)
        {
            return _dbContext.Vehicles.Any(x => x.ChassisNumber == chassisNumber);
        }

        public bool VehicleWithLicensePlateExists(string licensePlate)
        {
            return _dbContext.Vehicles.Any(x => x.LicensePlate == licensePlate);
        }
    }
}
