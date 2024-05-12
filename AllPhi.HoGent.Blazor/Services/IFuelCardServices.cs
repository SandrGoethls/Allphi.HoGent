using AllPhi.HoGent.Blazor.Dto;
using AllPhi.HoGent.Datalake.Data.Helpers;
using System.Runtime.InteropServices;

namespace AllPhi.HoGent.Blazor.Services
{
    public interface IFuelCardServices
    {
        Task<(bool, string message)> AddFuelCardAsync(FuelCardDto fuelCardDto);
        Task<bool> DeleteFuelCardAsync(Guid fuelCardId);
        Task<FuelCardListDto> GetAllFuelCardsAsync([Optional] string? sortBy, [Optional] bool isAscending, [Optional] FilterFuelCard filterFuelCard, [Optional] Pagination pagination);
        Task<bool> UpdateFuelCardAsync(FuelCardDto fuelCardDto);
    }
}
