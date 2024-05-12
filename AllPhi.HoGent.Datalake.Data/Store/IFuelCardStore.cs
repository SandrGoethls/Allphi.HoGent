using AllPhi.HoGent.Datalake.Data.Helpers;
using AllPhi.HoGent.Datalake.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AllPhi.HoGent.Datalake.Data.Store
{
    public interface IFuelCardStore
    {
        Task<FuelCard> GetFuelCardByFuelCardIdAsync(Guid fuelCardId);
        Task<(List<FuelCard>, int)> GetAllFuelCardsAsync(FilterFuelCard filterFuelCard, [Optional] string? sortBy, [Optional] bool isAscending, Pagination? pagination = null);
        Task AddFuelCard(FuelCard fuelCard);
        Task UpdateFuelCard(FuelCard fuelCard);
        Task RemoveFuelCard(Guid fuelCardId);
    }
}
