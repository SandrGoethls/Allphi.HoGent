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
    public interface IDriverVehicleStore
    {
        Task<(List<DriverVehicle>, int)> GetAllDriverVehicleAsync([Optional] string sortBy, [Optional] bool isAscending, Pagination? pagination = null);
        Task<List<DriverVehicle>> GetDriverWithConnectedVehicleByDriverId(Guid driverId);
        Task<List<DriverVehicle>> GetVehicleWithConnectedDriversByVehicleId(Guid vehicleId);
        Task<List<DriverVehicle>> GetDriverAndVehicleByDriverIdAndVehicleId(Guid driverId, Guid vehicleId);
        Task UpdateDriverWithVehiclesByDriverIdAndListOfVehicleIds(Guid driverId, List<Guid> newVehicleIds);
        Task UpdateVehicleWithDriversByFuelCardIdAndDriverIds(Guid vehicleId, List<Guid> newDriverIds);
    }
}
