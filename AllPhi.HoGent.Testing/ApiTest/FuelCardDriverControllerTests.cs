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
            var fuelCardDriverStoreMock = new Mock<IFuelCardDriverStore>();
            var controller = new FuelCardDriverController(fuelCardDriverStoreMock.Object);
            #endregion

            #region Act
            var result = await controller.GetAllFuelCardDrivers();
            #endregion

            #region Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var fuelCardDriverListDto = Assert.IsType<FuelCardDriverListDto>(actionResult.Value);
            Assert.NotNull(fuelCardDriverListDto.FuelCardDriverDtos);
            #endregion
        }

        [Fact]
        public async Task GetDriverWithFuelCardsByDriverId_ReturnsDriverWithFuelCards()
        {
            #region Arrange
            var driverId = Guid.NewGuid();
            var fuelCardDriverStoreMock = new Mock<IFuelCardDriverStore>();
            fuelCardDriverStoreMock.Setup(x => x.GetDriverWithConnectedFuelCardsByDriverId(driverId))
                                   .ReturnsAsync(new FuelCardDriverDto());
            var controller = new FuelCardDriverController(fuelCardDriverStoreMock.Object);
            #endregion


            #region Act
            var result = await controller.GetDriverWithFuelCardsByDriverId(driverId);
            #endregion

            #region Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var fuelCardDriverListDto = Assert.IsType<FuelCardDriverListDto>(actionResult.Value);
            Assert.NotNull(fuelCardDriverListDto);
            #endregion
        }

        [Fact]
        public async Task GetDriverWithConnectedFuelCardsByDriverId_ReturnsFuelCards_WhenCardsExist()
        {
            #region Arrange
            // var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            // var controller = new FuelCardDriverController(fuelCardDriverStoreMock.Object);
            // var driverId = Guid.NewGuid(); // Gebruik een specifieke Guid die brandstofkaarten zal teruggeven
            #endregion

            #region Act
            // var result = await controller.GetDriverWithConnectedFuelCardsByDriverId(driverId);
            #endregion

            #region Assert
            // var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            // var returnedFuelCards = Assert.IsType<List<FuelCardDriverDto>>(actionResult.Value);
            // Assert.NotEmpty(returnedFuelCards);
            #endregion
        }

        [Fact]
        public async Task GetDriverWithConnectedFuelCardsByDriverId_ReturnsNotFound_WhenNoCardsExist()
        {
            #region Arrange
            // var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            // var controller = new FuelCardDriverController(fuelCardDriverStoreMock.Object);
            // var driverId = Guid.Empty; // Gebruik een Guid die geen brandstofkaarten zal teruggeven
            #endregion

            #region Act
            // var result = await controller.GetDriverWithConnectedFuelCardsByDriverId(driverId);
            #endregion

            #region Assert
            // Assert.IsType<NotFoundObjectResult>(result.Result);
            #endregion
        }
    }
}
