using AllPhi.HoGent.Blazor.Dto;
using System.Runtime.InteropServices;

namespace AllPhi.HoGent.Blazor.Services
{
    public interface IDriverServices
    {
        Task<DriverListDto> GetAllDriversAsync([Optional] string? sortBy, [Optional] bool isAscending, [Optional] int pageNumber, [Optional] int pageSize);
    }
}
