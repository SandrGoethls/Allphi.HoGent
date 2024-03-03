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
        private readonly AllPhiDatalakeContext _dbContext;

        public FuelCardDriverStore(AllPhiDatalakeContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(List<FuelCardDriver>, int)> GetAllFuelCardDriverAsync([Optional] string sortBy, [Optional] bool isAscending, Pagination? pagination = null)
        {
            List<FuelCardDriver> fuelCardDrivers = new();

            IQueryable<FuelCardDriver> fuelCardDriverQuery = _dbContext.FuelCardDrivers.Include(x => x.Driver).Include(x => x.FuelCard);

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
            var driverWithFuelCards = _dbContext.FuelCardDrivers.Where(x => x.DriverId == driverId)
                                                                .Include(x => x.FuelCard)
                                                                .ThenInclude(x => x.FuelCardFuelTypes)
                                                                .ToList() ?? new List<FuelCardDriver>();
            return driverWithFuelCards;
        }

        public async Task<List<FuelCardDriver>> GetFuelCardWithConnectedDriversByFuelCardId(Guid fuelCardId)
        {
            var driverWithFuelCards = _dbContext.FuelCardDrivers.Where(x => x.FuelCardId == fuelCardId)
                                                               .Include(x => x.Driver)
                                                               .ToList() ?? new List<FuelCardDriver>();
            return driverWithFuelCards;
        }

        public async Task<List<FuelCardDriver>> GetDriverAndFuelCardByDriverIdAndFuelCardId(Guid driverId, Guid fuelCardId)
        {
            var driverWithFuelCards = _dbContext.FuelCardDrivers.Where(x => x.DriverId == driverId && x.FuelCardId == fuelCardId)
                                                               .Include(x => x.Driver)
                                                               .Include(x => x.FuelCard)
                                                               .ToList() ?? new List<FuelCardDriver>();
            return driverWithFuelCards;
        }

        public async Task UpdateDriverWithFuelCardsByDriverIdAndListOfFuelCardIds(Guid driverId, List<Guid> newFuelCardIds)
        {
            try
            {
                await _dbContext.Database.BeginTransactionAsync();

                var existingRelations = _dbContext.FuelCardDrivers.Where(fcd => fcd.DriverId == driverId);
                _dbContext.FuelCardDrivers.RemoveRange(existingRelations);
                await _dbContext.SaveChangesAsync();

                foreach (var fuelCardId in newFuelCardIds)
                {
                    _dbContext.FuelCardDrivers.Add(new FuelCardDriver { DriverId = driverId, FuelCardId = fuelCardId });
                }

                await _dbContext.SaveChangesAsync();
                await _dbContext.Database.CommitTransactionAsync();
            }
            catch
            {
                await _dbContext.Database.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task UpdateFuelCardWithDriversByFuelCardIdAndDriverIds(Guid fuelCardId, List<Guid> newDriverIds)
        {
            try
            {
                await _dbContext.Database.BeginTransactionAsync();

                var existingRelations = _dbContext.FuelCardDrivers.Where(fcd => fcd.FuelCardId == fuelCardId);
                _dbContext.FuelCardDrivers.RemoveRange(existingRelations);

                foreach (var driverId in newDriverIds)
                {
                    _dbContext.FuelCardDrivers.Add(new FuelCardDriver { DriverId = driverId, FuelCardId = fuelCardId });
                }

                await _dbContext.SaveChangesAsync();
                await _dbContext.Database.CommitTransactionAsync();
            }
            catch
            {
                await _dbContext.Database.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task RemoveDriverWithConnectedFuelCardsByDriverId(Guid driverId)
        {
            try
            {
                await _dbContext.Database.BeginTransactionAsync();

                var relationsToRemove = _dbContext.FuelCardDrivers.Where(fcd => fcd.DriverId == driverId);

                _dbContext.FuelCardDrivers.RemoveRange(relationsToRemove);

                await _dbContext.SaveChangesAsync();
                await _dbContext.Database.CommitTransactionAsync();
            }
            catch
            {
                await _dbContext.Database.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task RemoveFuelCardWithConnectedDriversByFuelCardId(Guid fuelCardId)
        {
            try
            {
                await _dbContext.Database.BeginTransactionAsync();

                var relationsToRemove = _dbContext.FuelCardDrivers.Where(fcd => fcd.FuelCardId == fuelCardId);

                _dbContext.FuelCardDrivers.RemoveRange(relationsToRemove);

                await _dbContext.SaveChangesAsync();
                await _dbContext.Database.CommitTransactionAsync();
            }
            catch
            {
                await _dbContext.Database.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
