using AllPhi.HoGent.Datalake.Data.Helpers;
using AllPhi.HoGent.Datalake.Data.Models;
using AllPhi.HoGent.Datalake.Data.Store;
using AllPhi.HoGent.RestApi.Controllers;
using AllPhi.HoGent.RestApi.Dto;
using AllPhi.HoGent.Testing.MockData;
using Microsoft.AspNetCore.Http.HttpResults;
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
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var fuelCardDriverListDto = Assert.IsType<FuelCardDriverListDto>(actionResult.Value);
            Assert.IsType<FuelCardDriverListDto>(fuelCardDriverListDto);
            #endregion
        }

        [Fact]
        public async Task GetDriverWithConnectedFuelCardsByDriverId_ReturnsFuelCards_WhenCardsExist()
        {
            #region Arrange
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            var controller = new FuelCardDriverController(fuelCardDriverStoreMock.Object);
            var driverId = new Guid("a7245037-c683-4f82-b261-5c053502ed93");
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
        public async Task GetDriverWithConnectedFuelCardsByDriverId_ReturnsBadRequestObjectResult_WhenNoCardsExist()
        {
            #region Arrange
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            var controller = new FuelCardDriverController(fuelCardDriverStoreMock.Object);
            var driverId = Guid.Empty;
            #endregion

            #region Act
            var result = await controller.GetDriverWithFuelCardsByDriverId(driverId);
            #endregion

            #region Assert
            Assert.IsType<BadRequestObjectResult>(result);
            #endregion
        }

        [Fact]
        public async Task GetFuelCardWithConnectedDriversByFuelCardId_ReturnsDrivers_WhenDriversExist()
        {
            #region Arrange
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            var controller = new FuelCardDriverController(fuelCardDriverStoreMock.Object);
            var fuelCardId = Guid.NewGuid();
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
        public async Task GetFuelCardWithConnectedDriversByFuelCardId_ReturnsBadRequestObjectResult_WhenNoDriversExist()
        {
            #region Arrange
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            var fuelCardId = Guid.Empty;
            var controller = new FuelCardDriverController(fuelCardDriverStoreMock.Object);
            #endregion

            #region Act           
            var result = await controller.GetFuelCardWithDriversByFuelCardId(fuelCardId);
            #endregion

            #region Assert
            Assert.IsType<BadRequestObjectResult>(result);
            #endregion
        }

        [Fact]
        public async Task UpdateDriverFuelCardsByDriverId_ReturnsOk_WhenDriverFuelCardsAreUpdated()
        {
            #region Arrange
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            var controller = new FuelCardDriverController(fuelCardDriverStoreMock.Object);
            var driverId = new Guid("a7245037-c683-4f82-b261-5c053502ed93");

            var newFuelCardIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
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
            var fuelCardId = Guid.NewGuid();
            var newDriverIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
            #endregion

            #region Act
            var result = await controller.UpdateFuelCardDriversByFuelCardId(fuelCardId, newDriverIds);
            #endregion

            #region Assert
            Assert.IsType<OkResult>(result);
            #endregion
        }

        [Fact]
        public async Task UpdateDriverFuelCardsByDriverId_ReturnsBadRequest_WhenDriverIdIsEmpty()
        {
            #region Arrange
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            var controller = new FuelCardDriverController(fuelCardDriverStoreMock.Object);
            var driverId = Guid.Empty;
            var newFuelCardIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
            #endregion

            #region Act
            var result = await controller.UpdateDriverFuelCardsByDriverId(driverId, newFuelCardIds);
            #endregion

            #region Assert
            Assert.IsType<NotFoundObjectResult>(result);
            #endregion
        }

        [Fact]
        public async Task UpdateFuelCardDriversByFuelCardId_ReturnsBadRequest_WhenFuelCardIdIsEmpty()
        {
            #region Arrange
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            var controller = new FuelCardDriverController(fuelCardDriverStoreMock.Object);
            var fuelCardId = Guid.Empty;
            var newDriverIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
            #endregion

            #region Act
            var result = await controller.UpdateFuelCardDriversByFuelCardId(fuelCardId, newDriverIds);
            #endregion

            #region Assert
            Assert.IsType<NotFoundObjectResult>(result);
            #endregion
        }

        [Fact]
        public async Task UpdateFuelCardDriversByFuelId_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            #region Arrange
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            fuelCardDriverStoreMock.Setup(x => x.UpdateFuelCardWithDriversByFuelCardIdAndDriverIds(It.IsAny<Guid>(), It.IsAny<List<Guid>>()))
                                                .Throws(new Exception());
            var controller = new FuelCardDriverController(fuelCardDriverStoreMock.Object);
            var fuelCardId = Guid.NewGuid();
            var newDriverIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
            #endregion

            #region Act
            var result = await controller.UpdateFuelCardDriversByFuelCardId(fuelCardId, newDriverIds);
            #endregion

            #region Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, (result as ObjectResult).StatusCode);
            #endregion
        }
    }
}
