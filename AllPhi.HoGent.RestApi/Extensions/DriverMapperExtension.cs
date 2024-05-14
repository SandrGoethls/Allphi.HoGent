using AllPhi.HoGent.Datalake.Data.Models;
using AllPhi.HoGent.RestApi.Controllers;
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
                DateOfBirth = driver.DateOfBirth,
                TypeOfDriverLicense = driver.TypeOfDriverLicense,
                CreatedAt = driver.CreatedAt,
                Status = driver.Status
            };
        }

        
        public static List<DriverDto> MapToDriverListDto(List<Driver> drivers)
        {
            return drivers.Select(d => new DriverDto
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName,
                City = d.City,
                Street = d.Street,
                HouseNumber = d.HouseNumber,
                PostalCode = d.PostalCode,
                RegisterNumber = d.RegisterNumber,
                DateOfBirth = d.DateOfBirth,
                TypeOfDriverLicense = d.TypeOfDriverLicense,
                CreatedAt = d.CreatedAt,
                Status = d.Status

            }).ToList();
            
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
                DateOfBirth = driverDto.DateOfBirth,
                TypeOfDriverLicense = driverDto.TypeOfDriverLicense,
                CreatedAt = driverDto.CreatedAt,
                Status = driverDto.Status
            };
        }




    }
}
