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
    public class DriverVehicleStore : IDriverVehicleStore
    {
        private readonly IDbContextFactory<AllPhiDatalakeContext> _dbContextFactory;

        public DriverVehicleStore(IDbContextFactory<AllPhiDatalakeContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<(List<DriverVehicle>, int)> GetAllDriverVehicleAsync([Optional] string sortBy, [Optional] bool isAscending, Pagination? pagination = null)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            List<DriverVehicle> driverVehicles = new();

            IQueryable<DriverVehicle> driverVehiclesQuery = dbContext.DriverVehicle.Include(x => x.Driver).Include(x => x.Vehicle);

            IQueryable<DriverVehicle> sortedDriverVehicles = sortBy switch
            {
                "driverid" => isAscending ? driverVehiclesQuery.OrderBy(x => x.DriverId) : driverVehiclesQuery.OrderByDescending(x => x.DriverId),
                "vehicleid" => isAscending ? driverVehiclesQuery.OrderBy(x => x.VehicleId) : driverVehiclesQuery.OrderByDescending(x => x.VehicleId),
                _ => driverVehiclesQuery
            };

            var totalItems = await sortedDriverVehicles.CountAsync();
            if (pagination != null)
            {
                sortedDriverVehicles = sortedDriverVehicles.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize);
            }

            driverVehicles = await sortedDriverVehicles.ToListAsync();

            return (driverVehicles, totalItems);
        }

        public async Task<List<DriverVehicle>> GetDriverWithConnectedVehicleByDriverId(Guid driverId)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            var driverWithVehicles = dbContext.DriverVehicle.Where(x => x.DriverId == driverId)
                                                                .Include(x => x.Vehicle)
                                                                .ToList() ?? new List<DriverVehicle>();
            return driverWithVehicles;
        }

        public async Task<List<DriverVehicle>> GetVehicleWithConnectedDriversByVehicleId(Guid vehicleId)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            var vehicleWithDrivers = dbContext.DriverVehicle.Where(x => x.VehicleId == vehicleId)
                                                               .Include(x => x.Driver)
                                                               .ToList() ?? new List<DriverVehicle>();
            return vehicleWithDrivers;
        }

        public async Task<List<DriverVehicle>> GetDriverAndVehicleByDriverIdAndVehicleId(Guid driverId, Guid vehicleId)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            var driverWithVehicle = dbContext.DriverVehicle.Where(x => x.DriverId == driverId && x.VehicleId == vehicleId)
                                                               .Include(x => x.Driver)
                                                               .Include(x => x.Vehicle)
                                                               .ToList() ?? new List<DriverVehicle>();
            return driverWithVehicle;
        }

        public async Task UpdateDriverWithVehiclesByDriverIdAndListOfVehicleIds(Guid driverId, List<Guid> newVehicleIds)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            try
            {
                await dbContext.Database.BeginTransactionAsync();

                var existingRelations = dbContext.DriverVehicle.Where(fcd => fcd.DriverId == driverId);
                dbContext.DriverVehicle.RemoveRange(existingRelations);
                await dbContext.SaveChangesAsync();

                foreach (var vehicleId in newVehicleIds)
                {
                    dbContext.DriverVehicle.Add(new DriverVehicle { DriverId = driverId, VehicleId = vehicleId });
                }

                await dbContext.SaveChangesAsync();
                await dbContext.Database.CommitTransactionAsync();
            }
            catch
            {
                await dbContext.Database.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task UpdateVehicleWithDriversByFuelCardIdAndDriverIds(Guid vehicleId, List<Guid> newDriverIds)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            try
            {
                await dbContext.Database.BeginTransactionAsync();

                var existingRelations = dbContext.DriverVehicle.Where(fcd => fcd.VehicleId == vehicleId);
                dbContext.DriverVehicle.RemoveRange(existingRelations);

                foreach (var driverId in newDriverIds)
                {
                    dbContext.DriverVehicle.Add(new DriverVehicle { DriverId = driverId, VehicleId = vehicleId });
                }

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
