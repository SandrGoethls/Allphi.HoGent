using AllPhi.HoGent.Datalake.Data.Models;
using AllPhi.HoGent.RestApi.Dto;
using Microsoft.AspNetCore.Mvc;

namespace AllPhi.HoGent.RestApi.Extensions
{
    public static class VehicleMapperExtension
    {
        
        internal static VehicleDto MapToVehicleDto(Vehicle vehicle)
        {
            return new VehicleDto
            {
                Id = vehicle.Id,
                ChassisNumber = vehicle.ChassisNumber,
                LicensePlate = vehicle.LicensePlate,
                CarBrand = vehicle.CarBrand,
                FuelType = vehicle.FuelType,
                TypeOfCar = vehicle.TypeOfCar,
                Color = vehicle.VehicleColor,
                NumberOfDoors = vehicle.NumberOfDoors,
                Status = vehicle.Status
            };
        }

        
        internal static VehicleListDto MapToVehicleListDto(List<Vehicle> vehicles, int count)
        {
            return new VehicleListDto
            {
                VehicleDtos = vehicles.Select(MapToVehicleDto).ToList(),
                TotalItems = count
            };
        }

        internal static Vehicle MapToVehicle(VehicleDto vehicleDto)
        {
            return new Vehicle
            {
                Id = vehicleDto.Id,
                ChassisNumber = vehicleDto.ChassisNumber,
                LicensePlate = vehicleDto.LicensePlate,
                CarBrand = vehicleDto.CarBrand,
                FuelType = vehicleDto.FuelType,
                TypeOfCar = vehicleDto.TypeOfCar,
                VehicleColor = vehicleDto.Color,
                NumberOfDoors = vehicleDto.NumberOfDoors,
                Status = vehicleDto.Status
            };
        }
    }
}
