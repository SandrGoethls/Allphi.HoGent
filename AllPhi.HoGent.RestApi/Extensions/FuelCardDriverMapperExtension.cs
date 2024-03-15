using AllPhi.HoGent.Datalake.Data.Models;
using AllPhi.HoGent.RestApi.Dto;
using Microsoft.AspNetCore.Mvc;

namespace AllPhi.HoGent.RestApi.Extensions
{
    public static class FuelCardDriverMapperExtension
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public static FuelCardDriverDto MapToFuelCardDriverDto(FuelCardDriver fuelCardDriver)
        {
            return new FuelCardDriverDto
            {
                DriverId = fuelCardDriver.DriverId,
                FuelCardId = fuelCardDriver.FuelCardId,
                Driver = fuelCardDriver.Driver,
                FuelCard = fuelCardDriver.FuelCard
            };
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public static FuelCardDriverListDto MapToFuelCardDriverListDto(List<FuelCardDriver> fuelCardDrivers)
        {
            return new FuelCardDriverListDto
            {
                FuelCardDriverDtos = fuelCardDrivers.Select(MapToFuelCardDriverDto).ToList(),
                TotalItems = fuelCardDrivers.Count
            };
        }
    }
}
