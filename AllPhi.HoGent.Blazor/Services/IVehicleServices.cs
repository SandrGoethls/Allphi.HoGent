using AllPhi.HoGent.Blazor.Dto;
using AllPhi.HoGent.Datalake.Data.Helpers;
using System.Runtime.InteropServices;

namespace AllPhi.HoGent.Blazor.Services
{
    public interface IVehicleServices
    {
        Task<(bool, string message)> AddFVehicleAsync(VehicleDto vehicleDto);
        Task<bool> DeleteVehicleAsync(Guid vehicleId);
        Task<VehicleListDto> GetAllVehicleAsync([Optional] string? sortBy, [Optional] bool isAscending, [Optional] FilterVehicle filterVehicle, [Optional] Pagination pagination);
        Task<VehicleDto> GetVehicleByIdAsync(Guid vehicleId);
        Task<bool> UpdateVehicleAsync(VehicleDto vehicleDto);
    }
}
