using AllPhi.HoGent.Blazor.Dto;
using System.Runtime.InteropServices;

namespace AllPhi.HoGent.Blazor.Services
{
    public interface IFuelCardServices
    {
        Task<(bool, string message)> AddFuelCardAsync(FuelCardDto fuelCardDto);
        Task<bool> DeleteFuelCardAsync(Guid fuelCardId);
        Task<FuelCardListDto> GetAllFuelCardsAsync([Optional] string? sortBy, [Optional] bool isAscending, [Optional] int pageNumber, [Optional] int pageSize);
        Task<bool> UpdateFuelCardAsync(FuelCardDto fuelCardDto);
    }
}
