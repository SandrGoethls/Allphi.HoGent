using AllPhi.HoGent.Blazor.Dto;

namespace AllPhi.HoGent.Blazor.Services
{
    public interface IFuelCardDriverServices
    {
        Task<(List<FuelCardDriverDto>, bool status, string message)> GetDriverWithConnectedFuelCardsByDriverId(Guid driverId);
        Task<(List<FuelCardDriverDto>, bool status, string message)> GetFuelCardWithConnectedDriversByFuelCardId(Guid fuelCardId);
        Task<(bool, string message)> UpdateDriverWithFuelCards(Guid driverId, List<Guid> fuelcardGuids);
        Task<(bool, string message)> UpdateFuelCardDriverAsync(Guid fuelCardId, List<Guid> driverGuids);
    }
}
