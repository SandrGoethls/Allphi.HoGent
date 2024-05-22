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

namespace AllPhi.HoGent.Testing.ApiTest
{
    public class VehicleControllerTest
    {
        [Fact]
        public async Task GetVehicleById_ReturnsOk()
        {
            #region Arrange
            var vehicleStore = VehicleStoreMock.GetVehicleStoreMock();
            var driverVehicleStore = DriverVehicleStoreMock.GetDriverVehicleStoreMock();
            var controller = new VehiclesController(vehicleStore.Object, driverVehicleStore.Object);
            var vehicleId = new Guid("a7245037-c683-4f82-b261-5c053502ed93");
            #endregion

            #region Act
            var result = await controller.GetVehicleById(vehicleId);
            #endregion

            #region Assert
            Assert.IsType<OkObjectResult>(result.Result);
            #endregion
        }

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
    }
}
