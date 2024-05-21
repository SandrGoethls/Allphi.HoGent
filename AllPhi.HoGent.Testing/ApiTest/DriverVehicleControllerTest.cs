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
    public class DriverVehicleControllerTest
    {
        [Fact]
        public async Task GetAllDriverVehicles_ReturnsListOfDriverVehicles()
        {
            #region Arrange
            var driverVehicleStoreMock = DriverVehicleStoreMock.GetDriverVehicleStoreMock();
            var controller = new DriverVehicleController(driverVehicleStoreMock.Object);
            #endregion

            #region Act
            var result = await controller.GetAllDriverVehicles();
            #endregion

            #region Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var driverVehicleListDto = Assert.IsType<DriverVehicleListDto>(actionResult.Value);
            Assert.IsType<DriverVehicleListDto>(driverVehicleListDto);
            #endregion
        }

        [Fact]
        public async Task GetDriverWithVehiclesByDriverId_ReturnsVehicles_WhenVehiclesExist()
        {
            #region Arrange
            var driverVehicleStoreMock = DriverVehicleStoreMock.GetDriverVehicleStoreMock();
            var controller = new DriverVehicleController(driverVehicleStoreMock.Object);
            var driverId = Guid.NewGuid();
            #endregion

            #region Act
            var result = await controller.GetDriverWithVehiclesByDriverId(driverId);
            #endregion

            #region Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedVehicles = Assert.IsType<List<DriverVehicleDto>>(actionResult.Value);
            Assert.NotEmpty(returnedVehicles);
            #endregion
        }

        [Fact]
        public async Task GetDriverWithVehiclesByDriverId_ReturnsBadRequest_WhenNoDriverIdExists()
        {
            #region Arrange
            var driverVehicleStoreMock = DriverVehicleStoreMock.GetDriverVehicleStoreMock();
            var controller = new DriverVehicleController(driverVehicleStoreMock.Object);
            var driverId = Guid.Empty;
            #endregion

            #region Act
            var result = await controller.GetDriverWithVehiclesByDriverId(driverId);
            #endregion

            #region Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            #endregion
        }

        [Fact]
        public async Task GetVehicleWithConnectedDriversByVehicleId_ReturnsDrivers_WhenDriversExist()
        {
            #region Arrange
            var driverVehicleStoreMock = DriverVehicleStoreMock.GetDriverVehicleStoreMock();
            var controller = new DriverVehicleController(driverVehicleStoreMock.Object);
            var vehicleId = Guid.NewGuid();
            #endregion

            #region Act
            var result = await controller.GetVehicleWithDriversByVehicleId(vehicleId);
            #endregion

            #region Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedDrivers = Assert.IsType<List<DriverVehicleDto>>(actionResult.Value);
            Assert.NotEmpty(returnedDrivers);
            #endregion
        }

        [Fact]
        public async Task GetVehicleWithConnectedDriversByVehicleId_ReturnsBadRequestObjectResult_WhenNoDriversExist()
        {
            #region Arrange
            var driverVehicleStoreMock = DriverVehicleStoreMock.GetDriverVehicleStoreMock();
            var controller = new DriverVehicleController(driverVehicleStoreMock.Object);
            var vehicleId = Guid.Empty;
            #endregion

            #region Act
            var result = await controller.GetVehicleWithDriversByVehicleId(vehicleId);
            #endregion

            #region Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            #endregion
        }

        [Fact]
        public async Task GetVehicleWithConnectedDriversByVehicleId_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            #region Arrange
            var driverVehicleStoreMock = DriverVehicleStoreMock.GetDriverVehicleStoreMock();
            var controller = new DriverVehicleController(driverVehicleStoreMock.Object);
            var vehicleId = Guid.NewGuid();
            driverVehicleStoreMock.Setup(x => x.GetVehicleWithConnectedDriversByVehicleId(It.IsAny<Guid>())).ThrowsAsync(new Exception());
            #endregion

            #region Act
            var result = await controller.GetVehicleWithDriversByVehicleId(vehicleId);
            #endregion

            #region Assert
            var actionResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, actionResult.StatusCode);
            #endregion
        }

        [Fact]
        public async Task UpdateDriverVehiclesByDriverId_ReturnsOk_WhenDriverAndVehicleIdsAreValid()
        {
            #region Arrange
            var driverVehicleStoreMock = DriverVehicleStoreMock.GetDriverVehicleStoreMock();
            var controller = new DriverVehicleController(driverVehicleStoreMock.Object);
            var driverId = Guid.NewGuid();
            var vehicleIds = new List<Guid> { Guid.NewGuid() };
            #endregion

            #region Act
            var result = await controller.UpdateDriverVehiclesByDriverId(driverId, vehicleIds);
            #endregion

            #region Assert
            var actionResult = Assert.IsType<OkResult>(result);
            #endregion
        }

        [Fact]
        public async Task UpdateDriverVehiclesByDriverId_ReturnsBadRequest_WhenDriverIdIsEmpty()
        {
            #region Arrange
            var driverVehicleStoreMock = DriverVehicleStoreMock.GetDriverVehicleStoreMock();
            var controller = new DriverVehicleController(driverVehicleStoreMock.Object);
            var driverId = Guid.Empty;
            var vehicleIds = new List<Guid> { Guid.NewGuid() };
            #endregion

            #region Act
            var result = await controller.UpdateDriverVehiclesByDriverId(driverId, vehicleIds);
            #endregion

            #region Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            #endregion
        }

        [Fact]
        public async Task UpdateVehicleWithDriversByFuelCardIdAndDriverIds_ReturnsOk_WhenVehicleAndDriverIdsAreValid()
        {
            #region Arrange
            var driverVehicleStoreMock = DriverVehicleStoreMock.GetDriverVehicleStoreMock();
            var controller = new DriverVehicleController(driverVehicleStoreMock.Object);
            var vehicleId = Guid.NewGuid();
            var driverIds = new List<Guid> { Guid.NewGuid() };
            #endregion

            #region Act
            var result = await controller.UpdateVehicleDriversByVehicleId(vehicleId, driverIds);
            #endregion

            #region Assert
            var actionResult = Assert.IsType<OkResult>(result);
            #endregion
        }

        [Fact]
        public async Task UpdateVehicleWithDriversByFuelCardIdAndDriverIds_ReturnsBadRequest_WhenVehicleIdIsEmpty()
        {
            #region Arrange
            var driverVehicleStoreMock = DriverVehicleStoreMock.GetDriverVehicleStoreMock();
            var controller = new DriverVehicleController(driverVehicleStoreMock.Object);
            var vehicleId = Guid.Empty;
            var driverIds = new List<Guid> { Guid.NewGuid() };
            #endregion

            #region Act
            var result = await controller.UpdateVehicleDriversByVehicleId(vehicleId, driverIds);
            #endregion

            #region Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            #endregion
        }

        [Fact]
        public async Task UpdateVehicleWithDriversByFuelCardIdAndDriverIds_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            #region Arrange
            var driverVehicleStoreMock = DriverVehicleStoreMock.GetDriverVehicleStoreMock();
            var controller = new DriverVehicleController(driverVehicleStoreMock.Object);
            var vehicleId = Guid.NewGuid();
            var driverIds = new List<Guid> { Guid.NewGuid() };
            driverVehicleStoreMock.Setup(x => x.UpdateVehicleWithDriversByFuelCardIdAndDriverIds(It.IsAny<Guid>(), It.IsAny<List<Guid>>())).ThrowsAsync(new Exception());
            #endregion

            #region Act
            var result = await controller.UpdateVehicleDriversByVehicleId(vehicleId, driverIds);
            #endregion

            #region Assert
            var actionResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, actionResult.StatusCode);
            #endregion
        }

        [Fact]
        public async Task UpdateDriverVehiclesByDriverId_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            #region Arrange
            var driverVehicleStoreMock = DriverVehicleStoreMock.GetDriverVehicleStoreMock();
            var controller = new DriverVehicleController(driverVehicleStoreMock.Object);
            var driverId = Guid.NewGuid();
            var vehicleIds = new List<Guid> { Guid.NewGuid() };
            driverVehicleStoreMock.Setup(x => x.UpdateDriverWithVehiclesByDriverIdAndListOfVehicleIds(It.IsAny<Guid>(), It.IsAny<List<Guid>>())).ThrowsAsync(new Exception());
            #endregion

            #region Act
            var result = await controller.UpdateDriverVehiclesByDriverId(driverId, vehicleIds);
            #endregion

            #region Assert
            var actionResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, actionResult.StatusCode);
            #endregion
        }
    }
}
