using AllPhi.HoGent.Datalake.Data.Helpers;
using AllPhi.HoGent.Datalake.Data.Models;
using AllPhi.HoGent.Datalake.Data.Models.Enums;
using AllPhi.HoGent.Datalake.Data.Store;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllPhi.HoGent.Testing.MockData
{
    public class FuelCardDriverStoreMock
    {
        public static Mock<IFuelCardDriverStore> GetFuelCardDriverStoreMock()
        {
            var mock = new Mock<IFuelCardDriverStore>();

            var mockFuelCardDrivers = new List<FuelCardDriver>
            {
                new FuelCardDriver
                {
                    DriverId = Guid.NewGuid(),
                    FuelCardId = Guid.NewGuid(),
                }
        };

            mock.Setup(m => m.GetDriverWithConnectedFuelCardsByDriverId(It.IsAny<Guid>())).ReturnsAsync(mockFuelCardDrivers);

            mock.Setup(m => m.GetDriverWithConnectedFuelCardsByDriverId(It.Is<Guid>(id => id == Guid.Empty))).ReturnsAsync(new List<FuelCardDriver>());

            mock.Setup(m => m.GetAllFuelCardDriverAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<Pagination?>()))
                .ReturnsAsync((mockFuelCardDrivers, mockFuelCardDrivers.Count));
            
            mock.Setup(m => m.GetFuelCardWithConnectedDriversByFuelCardId(It.IsAny<Guid>())).ReturnsAsync(mockFuelCardDrivers);

            mock.Setup(m => m.GetFuelCardWithConnectedDriversByFuelCardId(It.Is<Guid>(id => id == Guid.Empty))).ReturnsAsync(new List<FuelCardDriver>());

            mock.Setup(m => m.UpdateDriverWithFuelCardsByDriverIdAndListOfFuelCardIds(It.IsAny<Guid>(), It.IsAny<List<Guid>>())).Returns(Task.CompletedTask);

            mock.Setup(m => m.UpdateFuelCardWithDriversByFuelCardIdAndDriverIds(It.IsAny<Guid>(), It.IsAny<List<Guid>>()));

            return mock;
        }
    }
}
