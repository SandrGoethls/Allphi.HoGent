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
    public class FuelCardStore : IFuelCardStore
    {
        private readonly IDbContextFactory<AllPhiDatalakeContext> _dbContextFactory;

        public FuelCardStore(IDbContextFactory<AllPhiDatalakeContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<FuelCard> GetFuelCardByFuelCardIdAsync(Guid fuelCardId)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            return await dbContext.FuelCards.Include(x => x.FuelCardFuelTypes).FirstOrDefaultAsync(x => x.Id == fuelCardId);

        }

        public async Task<(List<FuelCard>, int)> GetAllFuelCardsAsync([Optional] string sortBy, [Optional] bool isAscending, Pagination? pagination = null)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            List<FuelCard> fuelCards = new();

            IQueryable<FuelCard> fuelCardsQuery = dbContext.FuelCards.Include(x => x.FuelCardFuelTypes);

            IQueryable<FuelCard> sortedFuelCards = sortBy switch
            {
                "id" => isAscending ? fuelCardsQuery.OrderBy(x => x.Id) : fuelCardsQuery.OrderByDescending(x => x.Id),
                "cardNumber" => isAscending ? fuelCardsQuery.OrderBy(x => x.CardNumber) : fuelCardsQuery.OrderByDescending(x => x.CardNumber),
                "pin" => isAscending ? fuelCardsQuery.OrderBy(x => x.Pin) : fuelCardsQuery.OrderByDescending(x => x.Pin),
                "validityDate" => isAscending ? fuelCardsQuery.OrderBy(x => x.ValidityDate) : fuelCardsQuery.OrderByDescending(x => x.ValidityDate),
                _ => fuelCardsQuery
            };

            var totalItems = await sortedFuelCards.CountAsync();
            if (pagination != null)
            {
                sortedFuelCards = sortedFuelCards.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize);
            }

            fuelCards = await sortedFuelCards.ToListAsync();

            return (fuelCards, totalItems);
        }

        public async Task AddFuelCard(FuelCard fuelCard)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            bool existingFuelCard = await dbContext.FuelCards.AnyAsync(x => x.CardNumber == fuelCard.CardNumber);

            if (!existingFuelCard)
            {
                try
                {
                    await dbContext.Database.BeginTransactionAsync();

                    await dbContext.FuelCards.AddAsync(fuelCard);

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
                throw new Exception("Fuelcard already exist!");
        }

        public async Task UpdateFuelCard(FuelCard fuelCard)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            var existingFuelCard = await dbContext.FuelCards.FindAsync(fuelCard.Id);
            if (existingFuelCard == null)
            {
                throw new Exception("FuelCard not found.");
            }

            try
            {
                await dbContext.Database.BeginTransactionAsync();

                var fuelCardTypes = dbContext.FuelCardFuelTypes.Where(x => x.FuelCardId == fuelCard.Id);
                dbContext.FuelCardFuelTypes.RemoveRange(fuelCardTypes);
                await dbContext.SaveChangesAsync();

                bool existingFuelCardNumber = await dbContext.FuelCards.AnyAsync(x => x.CardNumber == fuelCard.CardNumber && x.Id != fuelCard.Id);
                if (!existingFuelCardNumber)
                {
                    dbContext.Entry(existingFuelCard).CurrentValues.SetValues(fuelCard);

                    if (fuelCard.FuelCardFuelTypes != null && fuelCard.FuelCardFuelTypes.Any())
                    {
                        await dbContext.FuelCardFuelTypes.AddRangeAsync(fuelCard.FuelCardFuelTypes);
                    }

                    await dbContext.SaveChangesAsync();

                    await dbContext.Database.CommitTransactionAsync();
                }
                else
                {
                    await dbContext.Database.RollbackTransactionAsync();
                    throw new Exception("A fuelcard already exist with the same cardnumber!");
                }

            }
            catch
            {
                await dbContext.Database.RollbackTransactionAsync();
                throw;
            }

        }

        public async Task RemoveFuelCard(Guid fuelCardId)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            var existingFuelCard = await dbContext.FuelCards.FindAsync(fuelCardId);
            if (existingFuelCard == null)
            {
                throw new Exception("FuelCard not found.");
            }

            try
            {
                await dbContext.Database.BeginTransactionAsync();

                var relatedEntries = dbContext.FuelCardDrivers.Where(fcd => fcd.FuelCardId == fuelCardId);
                dbContext.FuelCardDrivers.RemoveRange(relatedEntries);

                dbContext.FuelCards.Remove(existingFuelCard);

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
