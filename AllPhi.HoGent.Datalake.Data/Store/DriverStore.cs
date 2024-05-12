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
        private readonly AllPhiDatalakeContext _dbContext;

        public DriverStore(AllPhiDatalakeContext dbContext)
        {

            _dbContext = dbContext;
        }

        public async Task<Driver> GetDriverByIdAsync(Guid driverId)
        {
            return await _dbContext.Drivers.FirstOrDefaultAsync(x => x.Id == driverId) ?? new();
        }

        public async Task<(List<Driver>, int)> GetAllDriversAsync([Optional] string sortBy, [Optional] bool isAscending, Pagination? pagination = null)
        {
            List<Driver> drivers = new();

            IQueryable<Driver> driverQuery = _dbContext.Drivers;

            IQueryable<Driver> sorteddrivers = sortBy switch
            {
                "firstname" => isAscending ? driverQuery.OrderBy(x => x.FirstName) : driverQuery.OrderByDescending(x => x.FirstName),
                "lastname" => isAscending ? driverQuery.OrderBy(x => x.LastName) : driverQuery.OrderByDescending(x => x.LastName),
                "city" => isAscending ? driverQuery.OrderBy(x => x.City) : driverQuery.OrderByDescending(x => x.City),
                "street" => isAscending ? driverQuery.OrderBy(x => x.Street) : driverQuery.OrderByDescending(x => x.Street),
                "housenumber" => isAscending ? driverQuery.OrderBy(x => x.HouseNumber) : driverQuery.OrderByDescending(x => x.HouseNumber),
                "registernumber" => isAscending ? driverQuery.OrderBy(x => x.RegisterNumber) : driverQuery.OrderByDescending(x => x.RegisterNumber),
                "typeofdriverlicense" => isAscending ? driverQuery.OrderBy(x => x.TypeOfDriverLicense) : driverQuery.OrderByDescending(x => x.TypeOfDriverLicense),
                "status" => isAscending ? driverQuery.OrderBy(x => x.Status) : driverQuery.OrderByDescending(x => x.Status),
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
            bool existingDriver = await _dbContext.Drivers.AnyAsync(x => x.RegisterNumber == driver.RegisterNumber);

            if (!existingDriver)
            {
                try
                {
                    await _dbContext.Database.BeginTransactionAsync();

                    await _dbContext.Drivers.AddAsync(driver);
                    await _dbContext.SaveChangesAsync();

                    await _dbContext.Database.CommitTransactionAsync();
                }
                catch (Exception ex)
                {
                    await _dbContext.Database.RollbackTransactionAsync();
                    throw;
                }
            }
            else
                throw new Exception("Driver already exist!");
        }

        public async Task UpdateDriver(Driver driver)
        {
            var existingDriver = await _dbContext.Drivers.FindAsync(driver.Id);

            if (existingDriver == null)
            {
                throw new Exception("Driver not found.");
            }

            try
            {
                await _dbContext.Database.BeginTransactionAsync();

                _dbContext.Entry(existingDriver).CurrentValues.SetValues(driver);

                await _dbContext.SaveChangesAsync();

                await _dbContext.Database.CommitTransactionAsync();
            }
            catch
            {
                await _dbContext.Database.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task RemoveDriver(Guid driverId)
        {
            var driverToRemove = await _dbContext.Drivers.FindAsync(driverId);

            if (driverToRemove == null)
            {
                throw new Exception("Driver not found.");
            }

            try
            {
                await _dbContext.Database.BeginTransactionAsync();

                var relatedEntries = _dbContext.FuelCardDrivers.Where(fcd => fcd.DriverId == driverId);
                _dbContext.FuelCardDrivers.RemoveRange(relatedEntries);

                _dbContext.Drivers.Remove(driverToRemove);

                await _dbContext.SaveChangesAsync();

                await _dbContext.Database.CommitTransactionAsync();
            }
            catch
            {
                await _dbContext.Database.RollbackTransactionAsync();
                throw;
            }
        }

        public bool DriverWithRegisterNumberExists(string registerNumber)
        {
            return _dbContext.Drivers.Any(x => x.RegisterNumber == registerNumber);
        }
    }
}
