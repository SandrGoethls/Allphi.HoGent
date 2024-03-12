using AllPhi.HoGent.Datalake.Data.Models;
using AllPhi.HoGent.RestApi.Dto;
using Microsoft.AspNetCore.Mvc;

namespace AllPhi.HoGent.RestApi.Extensions
{
    public static class DriverMapperExtensions
    {
        public static DriverDto MapToDriverDto(Driver driver)
        {
            return new DriverDto
            {
                Id = driver.Id,
                FirstName = driver.FirstName,
                LastName = driver.LastName,
                City = driver.City,
                Street = driver.Street,
                HouseNumber = driver.HouseNumber,
                PostalCode = driver.PostalCode,
                RegisterNumber = driver.RegisterNumber,
                TypeOfDriverLicense = driver.TypeOfDriverLicense,
                Status = driver.Status
            };
        }

        
        public static DriverListDto MapToDriverListDto(List<Driver> drivers, int count)
        {
            return new DriverListDto
            {
                DriverDtos = drivers.Select(MapToDriverDto).ToList(),
                TotalItems = count
            };
        }

       public static Driver MapToDriver(DriverDto driverDto)
        {
            return new Driver
            {
                Id = driverDto.Id,
                FirstName = driverDto.FirstName,
                LastName = driverDto.LastName,
                City = driverDto.City,
                Street = driverDto.Street,
                HouseNumber = driverDto.HouseNumber,
                PostalCode = driverDto.PostalCode,
                RegisterNumber = driverDto.RegisterNumber,
                TypeOfDriverLicense = driverDto.TypeOfDriverLicense,
                Status = driverDto.Status
            };
        }




    }
}
