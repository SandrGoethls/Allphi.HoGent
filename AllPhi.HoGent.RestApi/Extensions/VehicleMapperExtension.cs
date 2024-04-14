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


        internal static List<VehicleDto> MapToVehicleListDto(List<Vehicle> vehicles)
        {
            return vehicles.Select(v => new VehicleDto
            {

                Id = v.Id,
                ChassisNumber = v.ChassisNumber,
                LicensePlate = v.LicensePlate,
                CarBrand = v.CarBrand,
                FuelType = v.FuelType,
                TypeOfCar = v.TypeOfCar,
                Color = v.VehicleColor,
                NumberOfDoors = v.NumberOfDoors,
                Status = v.Status

            }).ToList();
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
