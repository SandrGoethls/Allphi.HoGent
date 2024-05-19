using AllPhi.HoGent.Datalake.Data.Store;
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
    public class FuelCardDriverControllerTests
    {
        [Fact]
        public async Task GetAllFuelCardDrivers_ReturnsListOfFuelCardDrivers()
        {
            #region Arrange
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            var controller = new FuelCardDriverController(fuelCardDriverStoreMock.Object);
            #endregion

            #region Act
            var result = await controller.GetAllFuelCardDrivers();
            #endregion

            #region Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var fuelCardDriverListDto = Assert.IsType<List<FuelCardDriverListDto>>(actionResult.Value);
            Assert.NotEmpty(fuelCardDriverListDto);
            #endregion
        }

        [Fact]
        public async Task GetDriverWithConnectedFuelCardsByDriverId_ReturnsFuelCards_WhenCardsExist()
        {
            #region Arrange
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            var controller = new FuelCardDriverController(fuelCardDriverStoreMock.Object);
            var driverId = Guid.NewGuid(); // Gebruik een specifieke Guid die brandstofkaarten zal teruggeven
            #endregion

            #region Act
            var result = await controller.GetDriverWithFuelCardsByDriverId(driverId);
            #endregion

            #region Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnedFuelCards = Assert.IsType<List<FuelCardDriverDto>>(actionResult.Value);
            Assert.NotEmpty(returnedFuelCards);
            #endregion
        }

        [Fact]
        public async Task GetDriverWithConnectedFuelCardsByDriverId_ReturnsNotFound_WhenNoCardsExist()
        {
            #region Arrange
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            var controller = new FuelCardDriverController(fuelCardDriverStoreMock.Object);
            var driverId = Guid.Empty; // Gebruik een Guid die geen brandstofkaarten zal teruggeven
            #endregion

            #region Act
            var result = await controller.GetDriverWithFuelCardsByDriverId(driverId);
            #endregion

            #region Assert
            Assert.IsType<NotFoundObjectResult>(result);
            #endregion
        }

        [Fact]
        public async Task GetFuelCardWithConnectedDriversByFuelCardId_ReturnsDrivers_WhenDriversExist()
        {
            #region Arrange
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            var controller = new FuelCardDriverController(fuelCardDriverStoreMock.Object);
            var fuelCardId = Guid.NewGuid(); // Gebruik een specifieke Guid die chauffeurs zal teruggeven
            #endregion

            #region Act
            var result = await controller.GetFuelCardWithDriversByFuelCardId(fuelCardId);
            #endregion

            #region Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnedDrivers = Assert.IsType<List<FuelCardDriverDto>>(actionResult.Value);
            Assert.NotEmpty(returnedDrivers);
            #endregion
        }

        [Fact]
        public async Task GetFuelCardWithConnectedDriversByFuelCardId_ReturnsNotFound_WhenNoDriversExist()
        {
            #region Arrange
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            var controller = new FuelCardDriverController(fuelCardDriverStoreMock.Object);
            var fuelCardId = Guid.Empty; // Gebruik een Guid die geen chauffeurs zal teruggeven
            #endregion

            #region Act
            var result = await controller.GetFuelCardWithDriversByFuelCardId(fuelCardId);
            #endregion

            #region Assert
            Assert.IsType<NotFoundObjectResult>(result);
            #endregion
        }

        [Fact]
        public async Task UpdateDriverFuelCardsByDriverId_ReturnsOk_WhenDriverFuelCardsAreUpdated()
        {
            #region Arrange
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            var controller = new FuelCardDriverController(fuelCardDriverStoreMock.Object);
            var driverId = Guid.NewGuid(); // Gebruik een specifieke Guid die brandstofkaarten zal updaten
            var newFuelCardIds = new List<Guid> { Guid.NewGuid() }; // Gebruik een lijst met specifieke Guids
            #endregion

            #region Act
            var result = await controller.UpdateDriverFuelCardsByDriverId(driverId, newFuelCardIds);
            #endregion

            #region Assert
            Assert.IsType<OkResult>(result);
            #endregion
        }

        [Fact]
        public async Task UpdateFuelCardDriversByFuelCardId_ReturnsOk_WhenFuelCardDriversAreUpdated()
        {
            #region Arrange
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            var controller = new FuelCardDriverController(fuelCardDriverStoreMock.Object);
            var fuelCardId = Guid.NewGuid(); // Gebruik een specifieke Guid die chauffeurs zal updaten
            var newDriverIds = new List<Guid> { Guid.NewGuid() }; // Gebruik een lijst met specifieke Guids
            #endregion

            #region Act
            var result = await controller.UpdateFuelCardDriversByFuelCardId(fuelCardId, newDriverIds);
            #endregion

            #region Assert
            Assert.IsType<OkResult>(result);
            #endregion
        }
    }
}
