using AllPhi.HoGent.Blazor.Dto;
using System.Runtime.InteropServices;

namespace AllPhi.HoGent.Blazor.Services
{
    public interface IVehicleServices
    {
        Task<(bool, string message)> AddFVehicleAsync(VehicleDto vehicleDto);
        Task<bool> DeleteVehicleAsync(Guid vehicleId);
        Task<VehicleListDto> GetAllVehicleAsync([Optional] string? sortBy, [Optional] bool isAscending, [Optional] int pageNumber, [Optional] int pageSize);
        Task<VehicleDto> GetVehicleByIdAsync(Guid vehicleId);
    }
}
