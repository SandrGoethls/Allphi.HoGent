using AllPhi.HoGent.Datalake.Data.Models.Enums;
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
    public class DriverStoreMock
    {
        public static Mock<IDriverStore> GetDriverStoreMock()
        {
            var mock = new Mock<IDriverStore>();

            var mockDriver = new Driver
            {
                Id = new Guid("a7245037-c683-4f82-b261-5c053502ed93"),
                FirstName = "Jan",
                LastName = "Jansen",
                Status = Status.Active,
                City = "Amsterdam",
                HouseNumber = "10",
                PostalCode = "1000AA",
                RegisterNumber = "95072308409",
                Street = "Hoofdstraat",
                TypeOfDriverLicense = TypeOfDriverLicense.B
            };

            var mockDriver_2 = new Driver
            {
                Id = new Guid("b7245037-c683-4f82-b261-5c053502ed93"),
                FirstName = "Tim",
                LastName = "Timsens",
                Status = Status.Active,
                City = "Rotterdam",
                HouseNumber = "20",
                PostalCode = "2000BB",
                RegisterNumber = "23052000609",
                Street = "Zijstraat",
                TypeOfDriverLicense = TypeOfDriverLicense.C
            };

            var mockDriver_3 = new Driver
            {
                Id = new Guid("c7245037-c683-4f82-b261-5c053502ed93"),
                FirstName = "Tom",
                LastName = "Tomsens",
                Status = Status.Active,
                City = "Lokeren",
                HouseNumber = "30",
                PostalCode = "3000AA",
                RegisterNumber = "24052002709",
                Street = "Koningstraat",
                TypeOfDriverLicense = TypeOfDriverLicense.C1
            };

            var mockDrivers = new List<Driver> { mockDriver };

            mock.Setup(m => m.GetDriverByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockDriver);

            mock.Setup(m => m.GetAllDriversAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<Pagination>()))
                                           .ReturnsAsync((mockDrivers, mockDrivers.Count));

            mock.Setup(m => m.AddDriver(It.IsAny<Driver>())).Returns(Task.CompletedTask);

            mock.Setup(m => m.UpdateDriver(It.IsAny<Driver>())).Returns(Task.CompletedTask);

            mock.Setup(m => m.RemoveDriver(It.IsAny<Guid>())).Returns(Task.CompletedTask);

            return mock;
        }
    }
}
