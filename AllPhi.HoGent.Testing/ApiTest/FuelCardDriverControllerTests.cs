using AllPhi.HoGent.RestApi.Controllers;
using AllPhi.HoGent.RestApi.Dto;
using AllPhi.HoGent.Testing.MockData;
using Microsoft.AspNetCore.Mvc;
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
        public async Task GetDriverWithConnectedFuelCardsByDriverId_ReturnsFuelCards_WhenCardsExist()
        {
            // Arrange
            //var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            //var controller = new FuelCardDriverController(fuelCardDriverStoreMock.Object);
            //var driverId = Guid.NewGuid(); // Gebruik een specifieke Guid die brandstofkaarten zal teruggeven

            // Act
            //var result = await controller.GetDriverWithConnectedFuelCardsByDriverId(driverId);

            // Assert
            //var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            //var returnedFuelCards = Assert.IsType<List<FuelCardDriverDto>>(actionResult.Value);
            //Assert.NotEmpty(returnedFuelCards);
        }

        [Fact]
        public async Task GetDriverWithConnectedFuelCardsByDriverId_ReturnsNotFound_WhenNoCardsExist()
        {
            // Arrange
            //var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            //var controller = new FuelCardDriverController(fuelCardDriverStoreMock.Object);
            //var driverId = Guid.Empty; // Gebruik een Guid die geen brandstofkaarten zal teruggeven

            // Act
            //var result = await controller.GetDriverWithConnectedFuelCardsByDriverId(driverId);

            // Assert
            //Assert.IsType<NotFoundObjectResult>(result.Result);
        }
    }
}
