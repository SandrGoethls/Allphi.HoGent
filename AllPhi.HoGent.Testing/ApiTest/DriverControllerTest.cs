using AllPhi.HoGent.Datalake.Data.Models.Enums;
using AllPhi.HoGent.Datalake.Data.Models;
using AllPhi.HoGent.RestApi.Controllers;
using AllPhi.HoGent.RestApi.Dto;
using AllPhi.HoGent.Testing.MockData;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using AllPhi.HoGent.Datalake.Data.Helpers;

namespace AllPhi.HoGent.Testing.ApiTest
{
    public class DriverControllerTest
    {
        // <========================================================>
        // <================== GET DRIVER BY ID ====================>
        // <========================================================>
        [Fact]
        public async Task GetDriverById_ReturnsCorrectItem_WhenDriverExists()
        {
            // Arrange
            var driverStoreMock = DriverStoreMock.GetDriverStoreMock();
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            var controller = new DriversController(driverStoreMock.Object, fuelCardDriverStoreMock.Object);
            var expectedDriverId = new Guid("a7245037-c683-4f82-b261-5c053502ed93");

            // Act
            var result = await controller.GetDriverById(expectedDriverId);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedDriver = Assert.IsType<DriverDto>(actionResult.Value);
            Assert.Equal(expectedDriverId, returnedDriver.Id);
        }

        [Fact]
        public async Task GetDriverById_ReturnsNotFound_WhenDriverDoesNotExist()
        {
            // Arrange
            var driverStoreMock = DriverStoreMock.GetDriverStoreMock();
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            driverStoreMock?.Setup(x => x.GetDriverByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Driver?)null);
            var controller = new DriversController(driverStoreMock.Object, fuelCardDriverStoreMock.Object);

            // Act
            var result = await controller.GetDriverById(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        // <========================================================>
        // <=================== GET ALL DRIVERS ====================>
        // <========================================================>
        [Fact]
        public async Task GetDrivers_ReturnsAllDrivers_WhenDriversExist()
        {
            // Arrange
            var driverStoreMock = DriverStoreMock.GetDriverStoreMock();
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            var controller = new DriversController(driverStoreMock.Object, fuelCardDriverStoreMock.Object);

            // Act
            var result = await controller.GetAllDrivers();

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedDrivers = Assert.IsType<DriverListDto>(actionResult.Value);
            Assert.NotEmpty(returnedDrivers.DriverDtos);
        }

        [Fact]
        public async Task GetDrivers_ReturnsNotFound_WhenNoDriversExist()
        {
            // Arrange
            var driverStoreMock = DriverStoreMock.GetDriverStoreMock();
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            driverStoreMock.Setup(m => m.GetAllDriversAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<Pagination>()))
                .ReturnsAsync((new List<Driver>(), 0));
            var controller = new DriversController(driverStoreMock.Object, fuelCardDriverStoreMock.Object);

            // Act
            var result = await controller.GetAllDrivers();

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        // <========================================================>
        // <====================== ADD DRIVER ======================>
        // <========================================================>
        [Fact]
        public async Task AddDriver_ReturnsOk_WhenDriverIsSuccessfullyAdded()
        {
            // Arrange
            var driverStoreMock = DriverStoreMock.GetDriverStoreMock();
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            var controller = new DriversController(driverStoreMock.Object, fuelCardDriverStoreMock.Object);
            var newDriverDto = new DriverDto
            {
                Id = Guid.NewGuid(),
                FirstName = "Bart",
                LastName = "Bartens",
                Status = Status.Active,
                City = "Zele",
                HouseNumber = "400",
                PostalCode = "4000NA",
                RegisterNumber = "95052329179",
                DateOfBirth = new DateTime(1995, 05, 23),
                Street = "Kortstraat",
                TypeOfDriverLicense = TypeOfDriverLicense.B
            };

            // Act
            var result = await controller.AddDriver(newDriverDto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal("Driver successfully added", okResult?.Value);
        }

        // <========================================================>
        // <==================== UPDATE DRIVER =====================>
        // <========================================================>
        [Fact]
        public async Task UpdateDriver_ReturnsOk_WhenDriverIsSuccessfullyUpdated()
        {
            // Arrange
            var driverStoreMock = DriverStoreMock.GetDriverStoreMock();
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            var controller = new DriversController(driverStoreMock.Object, fuelCardDriverStoreMock.Object);
            var driverToUpdate = new DriverDto
            {
                Id = new Guid("c7245037-c683-4f82-b261-5c053502ed93"),
                FirstName = "Bijgewerkt",
                LastName = "Bijgewerkt",
                Status = Status.Active,
                City = "Bijgewerkte",
                HouseNumber = "Bijgewerkt",
                PostalCode = "Bijgewerkt",
                RegisterNumber = "2222333",
                Street = "Bijgewerkt",
                TypeOfDriverLicense = TypeOfDriverLicense.D
            };

            // Act
            var result = await controller.UpdateDriver(driverToUpdate);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal("Driver successfully updated!", okResult?.Value);
        }

        // <========================================================>
        // <==================== DELETE DRIVER =====================>
        // <========================================================>
        [Fact]
        public async Task DeleteDriver_ReturnsOk_WhenDriverIsSuccessfullyDeleted()
        {
            // Arrange
            var driverStoreMock = DriverStoreMock.GetDriverStoreMock();
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            var controller = new DriversController(driverStoreMock.Object, fuelCardDriverStoreMock.Object);
            var driverId = new Guid("c7245037-c683-4f82-b261-5c053502ed93");

            // Act
            var result = await controller.DeleteDriver(driverId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal($"Driver with ID {driverId} successfully deleted.", okResult?.Value);
        }
    }
}
