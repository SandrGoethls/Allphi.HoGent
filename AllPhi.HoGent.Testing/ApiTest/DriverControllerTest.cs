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
            #region Arrange
            var driverStoreMock = DriverStoreMock.GetDriverStoreMock();
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            var controller = new DriversController(driverStoreMock.Object, fuelCardDriverStoreMock.Object);
            var expectedDriverId = new Guid("a7245037-c683-4f82-b261-5c053502ed93");
            #endregion

            #region Act
            var result = await controller.GetDriverById(expectedDriverId);
            #endregion

            #region Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedDriver = Assert.IsType<DriverDto>(actionResult.Value);
            Assert.Equal(expectedDriverId, returnedDriver.Id);
            #endregion
        }

        [Fact]
        public async Task GetDriverById_ReturnsNotFound_WhenDriverDoesNotExist()
        {
            #region Arrange
            var driverStoreMock = DriverStoreMock.GetDriverStoreMock();
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            driverStoreMock?.Setup(x => x.GetDriverByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Driver?)null);
            var controller = new DriversController(driverStoreMock.Object, fuelCardDriverStoreMock.Object);
            #endregion

            #region Act
            var result = await controller.GetDriverById(Guid.NewGuid());
            #endregion

            #region Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
            #endregion 
        }

        // <========================================================>
        // <=================== GET ALL DRIVERS ====================>
        // <========================================================>
        [Fact]
        public async Task GetDrivers_ReturnsAllDrivers_WhenDriversExist()
        {
            #region Arrange
            var driverStoreMock = DriverStoreMock.GetDriverStoreMock();
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            var controller = new DriversController(driverStoreMock.Object, fuelCardDriverStoreMock.Object);
            #endregion

            #region Act
            var result = await controller.GetAllDrivers();
            #endregion

            #region Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedDrivers = Assert.IsType<DriverListDto>(actionResult.Value);
            Assert.NotEmpty(returnedDrivers.DriverDtos);
            #endregion
        }

        [Fact]
        public async Task GetDrivers_ReturnsNotFound_WhenNoDriversExist()
        {
            #region Arrange
            var driverStoreMock = DriverStoreMock.GetDriverStoreMock();
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            driverStoreMock.Setup(m => m.GetAllDriversAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<Pagination>()))
                .ReturnsAsync((new List<Driver>(), 0));
            var controller = new DriversController(driverStoreMock.Object, fuelCardDriverStoreMock.Object);
            #endregion

            #region Act
            var result = await controller.GetAllDrivers();
            #endregion

            #region Assert
            Assert.IsType<NotFoundResult>(result.Result);
            #endregion
        }

        // <========================================================>
        // <====================== ADD DRIVER ======================>
        // <========================================================>
        [Fact]
        public async Task AddDriver_ReturnsBadRequest_WhenDriverWithRegisterNumberAlreadyExists()
        {
            #region Arrange
            var driverStoreMock = DriverStoreMock.GetDriverStoreMock();
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            driverStoreMock.Setup(x => x.DriverWithRegisterNumberExists(It.IsAny<string>())).Returns(true);
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
            #endregion

            #region Act
            var result = await controller.AddDriver(newDriverDto);
            #endregion

            #region Assert
            Assert.IsType<BadRequestObjectResult>(result);
            #endregion
        }

        [Fact]
        public async Task AddDriver_ReturnsBadRequest_WhenInvalidRegisterNumberIsProvided()
        {
            #region Arrange
            var driverStoreMock = DriverStoreMock.GetDriverStoreMock();
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            driverStoreMock.Setup(x => x.DriverWithRegisterNumberExists(It.IsAny<string>())).Returns(false);
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
                RegisterNumber = "95132329179",
                DateOfBirth = new DateTime(1995, 05, 23),
                Street = "Kortstraat",
                TypeOfDriverLicense = TypeOfDriverLicense.B
            };
            #endregion

            #region Act
            var result = await controller.AddDriver(newDriverDto);
            #endregion

            #region Assert
            Assert.IsType<BadRequestObjectResult>(result);
            #endregion
        }


        [Fact]
        public async Task AddDriver_ReturnsOk_WhenDriverIsSuccessfullyAdded()
        {
            #region Arrange
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
            #endregion

            #region Act
            var result = await controller.AddDriver(newDriverDto);
            #endregion

            #region Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal("Driver successfully added", okResult?.Value);
            #endregion
        }

        // <========================================================>
        // <==================== UPDATE DRIVER =====================>
        // <========================================================>
        [Fact]
        public async Task UpdateDriver_ReturnsOk_WhenDriverIsSuccessfullyUpdated()
        {
            #region Arrange
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
            #endregion

            #region Act
            var result = await controller.UpdateDriver(driverToUpdate);
            #endregion

            #region Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal("Driver successfully updated!", okResult?.Value);
            #endregion
        }

        // <========================================================>
        // <==================== DELETE DRIVER =====================>
        // <========================================================>
        [Fact]
        public async Task DeleteDriver_ReturnsOk_WhenDriverIsSuccessfullyDeleted()
        {
            #region Arrange
            var driverStoreMock = DriverStoreMock.GetDriverStoreMock();
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            var controller = new DriversController(driverStoreMock.Object, fuelCardDriverStoreMock.Object);
            var driverId = new Guid("c7245037-c683-4f82-b261-5c053502ed93");
            #endregion

            #region Act
            var result = await controller.DeleteDriver(driverId);
            #endregion

            #region Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal($"Driver with ID {driverId} successfully deleted.", okResult?.Value);
            #endregion
        }
    }
}
