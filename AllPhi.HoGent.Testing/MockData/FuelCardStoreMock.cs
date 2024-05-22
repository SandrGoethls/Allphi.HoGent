using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using AllPhi.HoGent.Datalake.Data.Models.Enums;
using AllPhi.HoGent.Datalake.Data.Models;
using AllPhi.HoGent.Datalake.Data.Store;
using AllPhi.HoGent.Datalake.Data.Helpers;

namespace AllPhi.HoGent.Testing.MockData
{
    public class FuelCardStoreMock
    {
        public static Mock<IFuelCardStore> GetFuelCardsStoreMock()
        {
            var mock = new Mock<IFuelCardStore>();

            var fuelcardMock_1 = new FuelCard
            {
                Id = new Guid("a7245037-c683-4f82-b261-5c053502ed93"),
                Pin = 1234,
                ValidityDate = DateTime.Now.AddYears(2),
                CreatedAt = DateTime.Now.AddYears(-2),
                CardNumber = "123456789",
                Status = Status.Active

            };

            var fuelCardMock_2 = new FuelCard
            {
                Id = Guid.NewGuid(),
                Pin = 4321,
                ValidityDate = DateTime.Now.AddDays(-1),
                CreatedAt = DateTime.Now.AddYears(-4),
                CardNumber = "987654321",
                Status = Status.Deactive
            };

            var fuelCardMock_3 = new FuelCard
            {
                Id = Guid.NewGuid(),
                Pin = 5678,
                ValidityDate = DateTime.Now.AddYears(4),
                CreatedAt = DateTime.Now,
                CardNumber = "213456789",
                Status = Status.Active,
            };

            mock.Setup(x => x.GetFuelCardByFuelCardIdAsync(It.IsAny<Guid>())).ReturnsAsync(fuelcardMock_1);

            mock.Setup(x => x.GetAllFuelCardsAsync(It.IsAny<FilterFuelCard>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<Pagination?>()))
                             .ReturnsAsync((new List<FuelCard> { fuelcardMock_1, fuelCardMock_2,}, 2));

            mock.Setup(x => x.AddFuelCard(It.IsAny<FuelCard>())).Returns(Task.CompletedTask);

            mock.Setup(x => x.UpdateFuelCard(It.IsAny<FuelCard>())).Returns(Task.CompletedTask);

            mock.Setup(x => x.RemoveFuelCard(It.IsAny<Guid>())).Returns(Task.CompletedTask);

            return mock;
        }
    }
}
