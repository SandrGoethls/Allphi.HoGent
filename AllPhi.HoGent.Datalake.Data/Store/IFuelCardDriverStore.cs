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
    public interface IFuelCardDriverStore
    {
        Task<(List<FuelCardDriver>, int)> GetAllFuelCardDriverAsync([Optional] string sortBy, [Optional] bool isAscending, Pagination? pagination = null);
        Task<List<FuelCardDriver>> GetDriverWithConnectedFuelCardsByDriverId(Guid driverId);
        Task<List<FuelCardDriver>> GetFuelCardWithConnectedDriversByFuelCardId(Guid fuelCardId);
        Task<List<FuelCardDriver>> GetDriverAndFuelCardByDriverIdAndFuelCardId(Guid driverId, Guid fuelCardId);
        Task UpdateDriverWithFuelCardsByDriverIdAndListOfFuelCardIds(Guid driverId, List<Guid> newFuelCardIds);
        Task UpdateFuelCardWithDriversByFuelCardIdAndDriverIds(Guid fuelCardId, List<Guid> newDriverIds);
        Task RemoveDriverWithConnectedFuelCardsByDriverId(Guid driverId);
        Task RemoveFuelCardWithConnectedDriversByFuelCardId(Guid fuelCardId);
    }
}
