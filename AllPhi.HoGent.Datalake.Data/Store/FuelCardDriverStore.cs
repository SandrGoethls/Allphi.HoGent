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
    public class FuelCardDriverStore : IFuelCardDriverStore
    {
        private readonly IDbContextFactory<AllPhiDatalakeContext> _dbContextFactory;

        public FuelCardDriverStore(IDbContextFactory<AllPhiDatalakeContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<(List<FuelCardDriver>, int)> GetAllFuelCardDriverAsync([Optional] string sortBy, [Optional] bool isAscending, Pagination? pagination = null)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            List<FuelCardDriver> fuelCardDrivers = new();

            IQueryable<FuelCardDriver> fuelCardDriverQuery = dbContext.FuelCardDrivers.Include(x => x.Driver).Include(x => x.FuelCard);

            IQueryable<FuelCardDriver> sortedFuelCardDrivers = sortBy switch
            {
                "driverid" => isAscending ? fuelCardDriverQuery.OrderBy(x => x.DriverId) : fuelCardDriverQuery.OrderByDescending(x => x.DriverId),
                "fuelid" => isAscending ? fuelCardDriverQuery.OrderBy(x => x.FuelCardId) : fuelCardDriverQuery.OrderByDescending(x => x.FuelCardId),
                _ => fuelCardDriverQuery
            };

            var totalItems = await sortedFuelCardDrivers.CountAsync();
            if (pagination != null)
            {
                sortedFuelCardDrivers = sortedFuelCardDrivers.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize);
            }

            fuelCardDrivers = await sortedFuelCardDrivers.ToListAsync();

            return (fuelCardDrivers, totalItems);
        }

        public async Task<List<FuelCardDriver>> GetDriverWithConnectedFuelCardsByDriverId(Guid driverId)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            var driverWithFuelCards = dbContext.FuelCardDrivers.Where(x => x.DriverId == driverId)
                                                                .Include(x => x.FuelCard)
                                                                .ThenInclude(x => x.FuelCardFuelTypes)
                                                                .ToList() ?? new List<FuelCardDriver>();
            return driverWithFuelCards;
        }

        public async Task<List<FuelCardDriver>> GetFuelCardWithConnectedDriversByFuelCardId(Guid fuelCardId)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            var driverWithFuelCards = dbContext.FuelCardDrivers.Where(x => x.FuelCardId == fuelCardId)
                                                               .Include(x => x.Driver)
                                                               .ToList() ?? new List<FuelCardDriver>();
            return driverWithFuelCards;
        }

        public async Task<List<FuelCardDriver>> GetDriverAndFuelCardByDriverIdAndFuelCardId(Guid driverId, Guid fuelCardId)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            var driverWithFuelCards = dbContext.FuelCardDrivers.Where(x => x.DriverId == driverId && x.FuelCardId == fuelCardId)
                                                               .Include(x => x.Driver)
                                                               .Include(x => x.FuelCard)
                                                               .ToList() ?? new List<FuelCardDriver>();
            return driverWithFuelCards;
        }

        public async Task UpdateDriverWithFuelCardsByDriverIdAndListOfFuelCardIds(Guid driverId, List<Guid> newFuelCardIds)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            try
            {
                await dbContext.Database.BeginTransactionAsync();

                var existingRelations = dbContext.FuelCardDrivers.Where(fcd => fcd.DriverId == driverId);
                dbContext.FuelCardDrivers.RemoveRange(existingRelations);
                await dbContext.SaveChangesAsync();

                foreach (var fuelCardId in newFuelCardIds)
                {
                    dbContext.FuelCardDrivers.Add(new FuelCardDriver { DriverId = driverId, FuelCardId = fuelCardId });
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

        public async Task UpdateFuelCardWithDriversByFuelCardIdAndDriverIds(Guid fuelCardId, List<Guid> newDriverIds)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            try
            {
                await dbContext.Database.BeginTransactionAsync();

                var existingRelations = dbContext.FuelCardDrivers.Where(fcd => fcd.FuelCardId == fuelCardId);
                dbContext.FuelCardDrivers.RemoveRange(existingRelations);

                foreach (var driverId in newDriverIds)
                {
                    dbContext.FuelCardDrivers.Add(new FuelCardDriver { DriverId = driverId, FuelCardId = fuelCardId });
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

        public async Task RemoveDriverWithConnectedFuelCardsByDriverId(Guid driverId)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            try
            {
                await dbContext.Database.BeginTransactionAsync();

                var relationsToRemove = dbContext.FuelCardDrivers.Where(fcd => fcd.DriverId == driverId);

                dbContext.FuelCardDrivers.RemoveRange(relationsToRemove);

                await dbContext.SaveChangesAsync();
                await dbContext.Database.CommitTransactionAsync();
            }
            catch
            {
                await dbContext.Database.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task RemoveFuelCardWithConnectedDriversByFuelCardId(Guid fuelCardId)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            try
            {
                await dbContext.Database.BeginTransactionAsync();

                var relationsToRemove = dbContext.FuelCardDrivers.Where(fcd => fcd.FuelCardId == fuelCardId);

                dbContext.FuelCardDrivers.RemoveRange(relationsToRemove);

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
