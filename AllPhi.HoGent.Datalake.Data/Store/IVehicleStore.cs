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
    public interface IVehicleStore
    {
        Task<Vehicle> GetVehicleByIdAsync(Guid vehicleId);
        Task<(List<Vehicle>, int)> GetAllVehiclesAsync([Optional] string? sortBy, [Optional] bool isAscending, Pagination? pagination = null);
        Task AddVehicle(Vehicle vehicle);
        Task UpdateVehicle(Vehicle vehicle);
        Task RemoveVehicle(Guid fuelCardId);
    }
}
