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
    public class DriverStore : IDriverStore
    {
        private readonly IDbContextFactory<AllPhiDatalakeContext> _dbContextFactory;

        public DriverStore(IDbContextFactory<AllPhiDatalakeContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<Driver> GetDriverByIdAsync(Guid driverId)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            return await dbContext.Drivers.FirstOrDefaultAsync(x => x.Id == driverId);
        }

        public async Task<(List<Driver>, int)> GetAllDriversAsync([Optional] string sortBy, [Optional] bool isAscending, Pagination? pagination = null)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            List<Driver> drivers = new();

            IQueryable<Driver> driverQuery = dbContext.Drivers;

            IQueryable<Driver> sorteddrivers = sortBy switch
            {
                "firstname" => isAscending ? driverQuery.OrderBy(x => x.FirstName) : driverQuery.OrderByDescending(x => x.FirstName),
                "lastname" => isAscending ? driverQuery.OrderBy(x => x.LastName) : driverQuery.OrderByDescending(x => x.LastName),
                "city" => isAscending ? driverQuery.OrderBy(x => x.City) : driverQuery.OrderByDescending(x => x.City),
                "street" => isAscending ? driverQuery.OrderBy(x => x.Street) : driverQuery.OrderByDescending(x => x.Street),
                _ => driverQuery
            };

            var totalItems = await sorteddrivers.CountAsync();
            if (pagination != null)
            {
                sorteddrivers = sorteddrivers.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize);
            }

            drivers = await sorteddrivers.ToListAsync();

            return (drivers, totalItems);
        }

        public async Task AddDriver(Driver driver)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            bool existingFuelCard = await dbContext.Drivers.AnyAsync(x => x.RegisterNumber == driver.RegisterNumber);

            if (!existingFuelCard)
            {
                try
                {
                    await dbContext.Database.BeginTransactionAsync();

                    await dbContext.Drivers.AddAsync(driver);
                    await dbContext.SaveChangesAsync();

                    await dbContext.Database.CommitTransactionAsync();
                }
                catch (Exception ex)
                {
                    await dbContext.Database.RollbackTransactionAsync();
                    throw;
                }
            }
            else
                throw new Exception("Driver already exist!");
        }

        public async Task UpdateDriver(Driver driver)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            var existingDriver = await dbContext.Drivers.FindAsync(driver.Id);

            if (existingDriver == null)
            {
                throw new Exception("Driver not found.");
            }

            try
            {
                await dbContext.Database.BeginTransactionAsync();

                dbContext.Entry(existingDriver).CurrentValues.SetValues(driver);

                await dbContext.SaveChangesAsync();

                await dbContext.Database.CommitTransactionAsync();
            }
            catch
            {
                await dbContext.Database.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task RemoveDriver(Guid driverId)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            var driverToRemove = await dbContext.Drivers.FindAsync(driverId);

            if (driverToRemove == null)
            {
                throw new Exception("Driver not found.");
            }

            try
            {
                await dbContext.Database.BeginTransactionAsync();

                var relatedEntries = dbContext.FuelCardDrivers.Where(fcd => fcd.DriverId == driverId);
                dbContext.FuelCardDrivers.RemoveRange(relatedEntries);

                dbContext.Drivers.Remove(driverToRemove);

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
