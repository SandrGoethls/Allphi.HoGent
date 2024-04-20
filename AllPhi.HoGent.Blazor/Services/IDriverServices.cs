using AllPhi.HoGent.Blazor.Dto;
using System.Runtime.InteropServices;

namespace AllPhi.HoGent.Blazor.Services
{
    public interface IDriverServices
    {
        Task<(bool, string message)> AddFDriverAsync(DriverDto driverDto);
        Task<bool> DeleteDriverAsync(Guid driverId);
        Task<DriverListDto> GetAllDriversAsync([Optional] string? sortBy, [Optional] bool isAscending, [Optional] int pageNumber, [Optional] int pageSize);
    }
}
