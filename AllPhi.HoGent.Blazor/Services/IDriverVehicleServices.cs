using AllPhi.HoGent.Blazor.Dto;
using AllPhi.HoGent.Datalake.Data.Helpers;
using AllPhi.HoGent.Datalake.Data.Models;
using System.Runtime.InteropServices;

namespace AllPhi.HoGent.Blazor.Services
{
    public interface IDriverVehicleServices
    {
        Task<(List<DriverVehicleDto>, bool status, string message)> GetDriverWithConnectedVehiclesByDriverId(Guid driverId);
        Task<(List<DriverVehicleDto>, bool status, string message)> GetVehicleWithConnectedDriversByVehicleId(Guid vehicleId);
        Task<(bool, string message)> UpdateDriverWithVehicles(Guid driverId, List<Guid> vehicleGuids);
        Task<(bool, string message)> UpdateVehicleWithDrivers(Guid vehicleId, List<Guid> driverGuids);
        Task<DriverVehicleListDto> GetAllDriverVehicleAsync([Optional] string sortBy, [Optional] bool isAscending, Pagination? pagination = null);
    }
}
