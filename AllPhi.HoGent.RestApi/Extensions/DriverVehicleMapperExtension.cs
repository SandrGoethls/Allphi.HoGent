using AllPhi.HoGent.Datalake.Data.Models;
using AllPhi.HoGent.RestApi.Dto;
using Microsoft.AspNetCore.Mvc;

namespace AllPhi.HoGent.RestApi.Extensions
{
    public static class DriverVehicleMapperExtension
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public static DriverVehicleDto MapToDriverVehicleDto(DriverVehicle driverVehicle)
        {
            return new DriverVehicleDto
            {
                DriverId = driverVehicle.DriverId,
                VehicleId = driverVehicle.VehicleId,
                Driver = driverVehicle.Driver,
                Vehicle = driverVehicle.Vehicle
            };
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public static DriverVehicleListDto MapToDriverVehicleListDto(List<DriverVehicle> driverVehicles, int count)
        {
            return new DriverVehicleListDto
            {
                DriverVehicleDtos = driverVehicles.Select(MapToDriverVehicleDto).ToList(),
                TotalItems = count
            };
        }
    }
}