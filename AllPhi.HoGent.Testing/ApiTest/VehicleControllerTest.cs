using AllPhi.HoGent.Datalake.Data.Helpers;
using AllPhi.HoGent.Datalake.Data.Models;
using AllPhi.HoGent.Datalake.Data.Models.Enums;
using AllPhi.HoGent.RestApi.Controllers;
using AllPhi.HoGent.RestApi.Dto;
using AllPhi.HoGent.Testing.MockData;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllPhi.HoGent.Testing.ApiTest
{
    public class VehicleControllerTest
    {
        [Fact]
        public async Task GetVehicleById_ReturnsNotFound()
        {
            #region Arrange
            var vehicleStore = VehicleStoreMock.GetVehicleStoreMock();
            var driverVehicleStore = DriverVehicleStoreMock.GetDriverVehicleStoreMock();
            var controller = new VehiclesController(vehicleStore.Object, driverVehicleStore.Object);
            var vehicleId = Guid.Empty;
            #endregion

            #region Act
            var result = await controller.GetVehicleById(vehicleId);
            #endregion

            #region Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
            #endregion
        }

        [Fact]
        public async Task GetAllVehicles_ReturnsOk()
        {
            #region Arrange
            var vehicleStore = VehicleStoreMock.GetVehicleStoreMock();
            var driverVehicleStore = DriverVehicleStoreMock.GetDriverVehicleStoreMock();
            var controller = new VehiclesController(vehicleStore.Object, driverVehicleStore.Object);
            #endregion

            #region Act
            var result = await controller.GetAllVehicles("1-ABC-123", "123456789");
            #endregion

            #region Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedVehicles = Assert.IsType<VehicleListDto>(actionResult.Value);
            Assert.NotEmpty(returnedVehicles.VehicleDtos);
            #endregion
        }

        [Fact]
        public async Task GetAllVehicles_ReturnsNotFound()
        {
            #region Arrange
            var vehicleStoreMock = VehicleStoreMock.GetVehicleStoreMock();
            var driverVehicleStoreMock = DriverVehicleStoreMock.GetDriverVehicleStoreMock();
            vehicleStoreMock.Setup(x => x.GetAllVehiclesAsync(It.IsAny<FilterVehicle>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<Pagination?>()))
                                .ReturnsAsync((new List<Vehicle> {}, 0));
            var controller = new VehiclesController(vehicleStoreMock.Object, driverVehicleStoreMock.Object);
            #endregion

            #region Act
            var result = await controller.GetAllVehicles(null, null);
            #endregion

            #region Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
            #endregion
        }

        [Fact]
        public async Task GetAllVehicles_ReturnsBadRequest()
        {
            #region Arrange
            var vehicleStoreMock = VehicleStoreMock.GetVehicleStoreMock();
            var driverVehicleStoreMock = DriverVehicleStoreMock.GetDriverVehicleStoreMock();
            vehicleStoreMock.Setup(x => x.GetAllVehiclesAsync(It.IsAny<FilterVehicle>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<Pagination?>()))
                                .ThrowsAsync(new ArgumentNullException());
            var controller = new VehiclesController(vehicleStoreMock.Object, driverVehicleStoreMock.Object);
            #endregion

            #region Act
            var result = await controller.GetAllVehicles(null, null);
            #endregion

            #region Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
            #endregion
        }

        [Fact]
        public async Task AddVehicle_ReturnsOk()
        {
            #region Arrange
            var vehicleStore = VehicleStoreMock.GetVehicleStoreMock();
            var driverVehicleStore = DriverVehicleStoreMock.GetDriverVehicleStoreMock();
            var controller = new VehiclesController(vehicleStore.Object, driverVehicleStore.Object);
            var vehicle = new VehicleDto
            {
                ChassisNumber = "123456789",
                LicensePlate = "1-ABC-123",
                CarBrand = CarBrand.Bmw,
                NumberOfDoors = NumberOfDoors.FiveDoors,
                FuelType = FuelType.Benzine,
                TypeOfCar = TypeOfCar.PassangerCar,
                Status = Status.Active,
                InspectionDate = DateTime.Now.AddYears(-1),
                CreatedAt = DateTime.Now.AddYears(-1)
            };
            #endregion

            #region Act
            var result = await controller.AddVehicle(vehicle);
            #endregion

            #region Assert
            Assert.IsType<OkResult>(result);
            #endregion
        }

        [Fact]
        public async Task AddVehicle_ReturnsBadRequest_WhenVehicleIsNull()
        {
            #region Arrange
            var vehicleStoreMock = VehicleStoreMock.GetVehicleStoreMock();
            var driverVehicleStoreMock = DriverVehicleStoreMock.GetDriverVehicleStoreMock();
            var controller = new VehiclesController(vehicleStoreMock.Object, driverVehicleStoreMock.Object);
       
            #endregion

            #region Act
            var result = await controller.AddVehicle(null);
            #endregion

            #region Assert
            Assert.IsType<BadRequestObjectResult>(result);
            #endregion
        }

        [Fact]
        public async Task AddVehicle_ReturnsBadRequest_WhenVehicleWithChassisNumberExists()
        {
            #region Arrange
            var vehicleStoreMock = VehicleStoreMock.GetVehicleStoreMock();
            var driverVehicleStoreMock = DriverVehicleStoreMock.GetDriverVehicleStoreMock();
            vehicleStoreMock.Setup(x => x.VehicleWithChassisNumberExists(It.IsAny<string>())).Returns(true);
            var controller = new VehiclesController(vehicleStoreMock.Object, driverVehicleStoreMock.Object);
            var vehicle = new VehicleDto
            {
                ChassisNumber = "123456789",
                LicensePlate = "1-ABC-123",
                CarBrand = CarBrand.Bmw,
                NumberOfDoors = NumberOfDoors.FiveDoors,
                FuelType = FuelType.Benzine,
                TypeOfCar = TypeOfCar.PassangerCar,
                Status = Status.Active,
                InspectionDate = DateTime.Now.AddYears(-1),
                CreatedAt = DateTime.Now.AddYears(-1)
            };
            #endregion

            #region Act
            var result = await controller.AddVehicle(vehicle);
            #endregion

            #region Assert
            Assert.IsType<BadRequestObjectResult>(result);
            #endregion
        }

        [Fact]
        public async Task AddVehicle_ReturnsBadRequest_WhenVehicleWithLicensePlateExists()
        {
            #region Arrange
            var vehicleStoreMock = VehicleStoreMock.GetVehicleStoreMock();
            var driverVehicleStoreMock = DriverVehicleStoreMock.GetDriverVehicleStoreMock();
            vehicleStoreMock.Setup(x => x.VehicleWithLicensePlateExists(It.IsAny<string>())).Returns(true);
            var controller = new VehiclesController(vehicleStoreMock.Object, driverVehicleStoreMock.Object);
            var vehicle = new VehicleDto
            {
                ChassisNumber = "123456789",
                LicensePlate = "1-ABC-123",
                CarBrand = CarBrand.Bmw,
                NumberOfDoors = NumberOfDoors.FiveDoors,
                FuelType = FuelType.Benzine,
                TypeOfCar = TypeOfCar.PassangerCar,
                Status = Status.Active,
                InspectionDate = DateTime.Now.AddYears(-1),
                CreatedAt = DateTime.Now.AddYears(-1)
            };
            #endregion

            #region Act
            var result = await controller.AddVehicle(vehicle);
            #endregion

            #region Assert
            Assert.IsType<BadRequestObjectResult>(result);
            #endregion
        }

        [Fact]
        public async Task UpdateVehicle_ReturnsOk_WhenVehicleHasBeenUpdated()
        {
            #region Arrange
            var vehicleStore = VehicleStoreMock.GetVehicleStoreMock();
            var driverVehicleStore = DriverVehicleStoreMock.GetDriverVehicleStoreMock();
            var controller = new VehiclesController(vehicleStore.Object, driverVehicleStore.Object);
            var vehicle = new VehicleDto
            {
                Id = new Guid("a7245037-c683-4f82-b261-5c053502ed93"),
                ChassisNumber = "123456789",
                LicensePlate = "1-ABC-123",
                CarBrand = CarBrand.Bmw,
                NumberOfDoors = NumberOfDoors.FiveDoors,
                FuelType = FuelType.Benzine,
                TypeOfCar = TypeOfCar.PassangerCar,
                Status = Status.Active,
                InspectionDate = DateTime.Now.AddYears(-1),
                CreatedAt = DateTime.Now.AddYears(-1)
            };
            #endregion

            #region Act
            var result = await controller.UpdateVehicle(vehicle);
            #endregion

            #region Assert
            Assert.IsType<OkResult>(result);
            #endregion
        }

        [Fact]
        public async Task DeleteVehicle_ReturnsOk_WhenVehicleHasBeenDeleted()
        {
            #region Arrange
            var vehicleStore = VehicleStoreMock.GetVehicleStoreMock();
            var driverVehicleStore = DriverVehicleStoreMock.GetDriverVehicleStoreMock();
            var controller = new VehiclesController(vehicleStore.Object, driverVehicleStore.Object);
            var vehicleId = new Guid("a7245037-c683-4f82-b261-5c053502ed93");
            #endregion

            #region Act
            var result = await controller.DeleteVehicle(vehicleId);
            #endregion

            #region Assert
            Assert.IsType<OkResult>(result);
            #endregion
        }

        [Fact]
        public async Task DeleteVehicle_ReturnsNotFound_WhenVehicleDoesNotExist()
        {
            #region Arrange
            var vehicleStoreMock = VehicleStoreMock.GetVehicleStoreMock();
            var driverVehicleStoreMock = DriverVehicleStoreMock.GetDriverVehicleStoreMock();
            vehicleStoreMock.Setup(x => x.RemoveVehicle(It.IsAny<Guid>())).ThrowsAsync(new InvalidOperationException());
            var controller = new VehiclesController(vehicleStoreMock.Object, driverVehicleStoreMock.Object);
            var vehicleId = Guid.Empty;
            #endregion

            #region Act
            var result = await controller.DeleteVehicle(vehicleId);
            #endregion

            #region Assert
            Assert.IsType<NotFoundObjectResult>(result);
            #endregion
        }

        [Fact]
        public async Task DeleteVehicle_ReturnsBadRequest_WhenVehicleIdIsEmpty()
        {
            #region Arrange
            var vehicleStoreMock = VehicleStoreMock.GetVehicleStoreMock();
            var driverVehicleStoreMock = DriverVehicleStoreMock.GetDriverVehicleStoreMock();
            var controller = new VehiclesController(vehicleStoreMock.Object, driverVehicleStoreMock.Object);
            var vehicleId = Guid.Empty;
            #endregion

            #region Act
            var result = await controller.DeleteVehicle(vehicleId);
            #endregion

            #region Assert
            Assert.IsType<NotFoundObjectResult>(result);
            #endregion
        }
    }
}
