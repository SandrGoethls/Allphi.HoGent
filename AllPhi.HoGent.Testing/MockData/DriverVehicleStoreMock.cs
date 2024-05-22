using AllPhi.HoGent.Datalake.Data.Models;
using AllPhi.HoGent.Datalake.Data.Store;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AllPhi.HoGent.Datalake.Data.Helpers;

namespace AllPhi.HoGent.Testing.MockData
{
    public class DriverVehicleStoreMock
    {
        public static Mock<IDriverVehicleStore> GetDriverVehicleStoreMock() 
        {
            var mock = new Mock<IDriverVehicleStore>();

            var mockDriverVehicles = new List<DriverVehicle>
            {
                new DriverVehicle
                {
                    DriverId = Guid.NewGuid(),
                    VehicleId = Guid.NewGuid(),
                }
            };

            mock.Setup(x => x.GetAllDriverVehicleAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<Pagination>()))
                .ReturnsAsync((mockDriverVehicles, mockDriverVehicles.Count));

            mock.Setup(x => x.GetDriverWithConnectedVehicleByDriverId(It.Is<Guid>(id => id == Guid.Empty))).ReturnsAsync(new List<DriverVehicle>());

            mock.Setup(x => x.GetDriverWithConnectedVehicleByDriverId(It.IsAny<Guid>())).ReturnsAsync(mockDriverVehicles);
            
            mock.Setup(x => x.GetVehicleWithConnectedDriversByVehicleId(It.IsAny<Guid>())).ReturnsAsync(mockDriverVehicles);

            mock.Setup(x => x.GetVehicleWithConnectedDriversByVehicleId(It.Is<Guid>(id => id == Guid.Empty))).ReturnsAsync(new List<DriverVehicle>());

            mock.Setup(x => x.UpdateDriverWithVehiclesByDriverIdAndListOfVehicleIds(It.IsAny<Guid>(), It.IsAny<List<Guid>>()))
                                                                                    .Returns(Task.CompletedTask);

            mock.Setup(x => x.UpdateVehicleWithDriversByFuelCardIdAndDriverIds(It.IsAny<Guid>(), It.IsAny<List<Guid>>()))
                                                                                    .Returns(Task.CompletedTask);

            return mock;
        }
    }
}
