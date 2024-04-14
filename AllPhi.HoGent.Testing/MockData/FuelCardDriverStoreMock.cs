using AllPhi.HoGent.Datalake.Data.Models;
using AllPhi.HoGent.Datalake.Data.Store;
using Moq;
using System;
using System.Collections.Generic;
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

            // Voor het scenario waar geen brandstofkaarten gevonden worden
            mock.Setup(m => m.GetDriverWithConnectedFuelCardsByDriverId(It.Is<Guid>(id => id == Guid.Empty))).ReturnsAsync(new List<FuelCardDriver>());

            return mock;
        }
    }
}
