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
        private readonly AllPhiDatalakeContext _dbContext;
        public FuelCardStore(AllPhiDatalakeContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<FuelCard> GetFuelCardByFuelCardIdAsync(Guid fuelCardId)
        {
            return await _dbContext.FuelCards.Include(x => x.FuelCardFuelTypes).FirstOrDefaultAsync(x => x.Id == fuelCardId);

        }

        public async Task<(List<FuelCard>, int)> GetAllFuelCardsAsync(FilterFuelCard filterFuelCard, [Optional] string sortBy, [Optional] bool isAscending, Pagination? pagination = null)
        {
            List<FuelCard> fuelCards = new();

            IQueryable<FuelCard> fuelCardsQuery = _dbContext.FuelCards.Include(x => x.FuelCardFuelTypes);

            IQueryable<FuelCard> sortedFuelCards = sortBy switch
            {
                "status" => isAscending ? fuelCardsQuery.OrderBy(x => x.Status) : fuelCardsQuery.OrderByDescending(x => x.Status),
                "cardnumber" => isAscending ? fuelCardsQuery.OrderBy(x => x.CardNumber) : fuelCardsQuery.OrderByDescending(x => x.CardNumber),
                "pin" => isAscending ? fuelCardsQuery.OrderBy(x => x.Pin) : fuelCardsQuery.OrderByDescending(x => x.Pin),
                "validityDate" => isAscending ? fuelCardsQuery.OrderBy(x => x.ValidityDate) : fuelCardsQuery.OrderByDescending(x => x.ValidityDate),
                _ => fuelCardsQuery
            };

            if (filterFuelCard != null)
            {
                if (!string.IsNullOrEmpty(filterFuelCard.SearchByCardNumber))
                    sortedFuelCards = sortedFuelCards.Where(x => x.CardNumber.Contains(filterFuelCard.SearchByCardNumber));
            }

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
            bool existingFuelCard = await _dbContext.FuelCards.AnyAsync(x => x.CardNumber == fuelCard.CardNumber);

            if (!existingFuelCard)
            {
                try
                {
                    await _dbContext.Database.BeginTransactionAsync();

                    await _dbContext.FuelCards.AddAsync(fuelCard);

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
                throw new Exception("Fuelcard already exist!");
        }

        public async Task UpdateFuelCard(FuelCard fuelCard)
        {
            var existingFuelCard = await _dbContext.FuelCards.FindAsync(fuelCard.Id);
            if (existingFuelCard == null)
            {
                throw new Exception("FuelCard not found.");
            }

            try
            {
                await _dbContext.Database.BeginTransactionAsync();

                var fuelCardTypes = _dbContext.FuelCardFuelTypes.Where(x => x.FuelCardId == fuelCard.Id);
                _dbContext.FuelCardFuelTypes.RemoveRange(fuelCardTypes);
                await _dbContext.SaveChangesAsync();

                bool existingFuelCardNumber = await _dbContext.FuelCards.AnyAsync(x => x.CardNumber == fuelCard.CardNumber && x.Id != fuelCard.Id);
                if (!existingFuelCardNumber)
                {
                    _dbContext.Entry(existingFuelCard).CurrentValues.SetValues(fuelCard);

                    if (fuelCard.FuelCardFuelTypes != null && fuelCard.FuelCardFuelTypes.Any())
                    {
                        await _dbContext.FuelCardFuelTypes.AddRangeAsync(fuelCard.FuelCardFuelTypes);
                    }

                    await _dbContext.SaveChangesAsync();

                    await _dbContext.Database.CommitTransactionAsync();
                }
                else
                {
                    await _dbContext.Database.RollbackTransactionAsync();
                    throw new Exception("A fuelcard already exist with the same cardnumber!");
                }

            }
            catch
            {
                await _dbContext.Database.RollbackTransactionAsync();
                throw;
            }

        }

        public async Task RemoveFuelCard(Guid fuelCardId)
        {
            var existingFuelCard = await _dbContext.FuelCards.FindAsync(fuelCardId);
            if (existingFuelCard == null)
            {
                throw new Exception("FuelCard not found.");
            }

            try
            {
                await _dbContext.Database.BeginTransactionAsync();

                var relatedEntries = _dbContext.FuelCardDrivers.Where(fcd => fcd.FuelCardId == fuelCardId);
                _dbContext.FuelCardDrivers.RemoveRange(relatedEntries);

                _dbContext.FuelCards.Remove(existingFuelCard);

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
