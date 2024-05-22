using AllPhi.HoGent.Datalake.Data.Models.Enums;
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
using Moq;
using AllPhi.HoGent.Datalake.Data.Helpers;

namespace AllPhi.HoGent.Testing.ApiTest
{
    public class FuelCardControllerTest
    {
        [Fact]
        public async Task GetFuelCardById_WithValidId_ReturnsCorrectFuelCard()
        {
            #region Arrange
            var fuelCardStoreMock = FuelCardStoreMock.GetFuelCardsStoreMock();
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            var fuelCardController = new FuelCardsController(fuelCardStoreMock.Object, fuelCardDriverStoreMock.Object);
            var expectedFuelcardId = new Guid("a7245037-c683-4f82-b261-5c053502ed93");
            #endregion

            #region Act
            var result = await fuelCardController.GetFuelCardById(expectedFuelcardId);
            #endregion

            #region Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedFuelCard = Assert.IsType<FuelCardDto>(actionResult.Value);
            Assert.Equal(expectedFuelcardId, returnedFuelCard.Id);
            #endregion
        }

        [Fact]
        public async Task GetFuelCardById_WithInvalidId_ReturnsNotFound()
        {
            #region Arrange
            var FuelcardId = Guid.Empty;
            var fuelCardStoreMock = FuelCardStoreMock.GetFuelCardsStoreMock();
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            fuelCardStoreMock?.Setup(store => store.GetFuelCardByFuelCardIdAsync(FuelcardId))
                          .ReturnsAsync((FuelCard)null);
            var fuelCardController = new FuelCardsController(fuelCardStoreMock.Object, fuelCardDriverStoreMock.Object);
            #endregion

            #region Act
            var result = await fuelCardController.GetFuelCardById(FuelcardId);
            #endregion

            #region Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
            #endregion
        }

        [Fact]
        public async Task GetFuelCards_ReturnsAllFuelCards_WhenFuelCardsExist()
        {
            #region Arrange
            var fuelCardStoreMock = FuelCardStoreMock.GetFuelCardsStoreMock();
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            var fuelCardController = new FuelCardsController(fuelCardStoreMock.Object, fuelCardDriverStoreMock.Object);
            #endregion

            #region Act
            var result = await fuelCardController.GetAllFuelCards(null, null);
            #endregion

            #region Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedFuelCards = Assert.IsType<FuelCardListDto>(actionResult.Value);
            Assert.NotEmpty(returnedFuelCards.FuelCardDtos);
            #endregion
        }

        [Fact]
        public async Task GetFuelCards_ReturnsNotFound_WhenNoFuelCardsExist()
        {
            #region Arrange
            var fuelCardStoreMock = FuelCardStoreMock.GetFuelCardsStoreMock();
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            fuelCardStoreMock?.Setup(store => store.GetAllFuelCardsAsync(It.IsAny<FilterFuelCard>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<Pagination?>()))
                          .ReturnsAsync((new List<FuelCard>(), 0));
            var fuelCardController = new FuelCardsController(fuelCardStoreMock.Object, fuelCardDriverStoreMock.Object);
            #endregion

            #region Act
            var result = await fuelCardController.GetAllFuelCards(null, null);
            #endregion

            #region Assert
            Assert.IsType<NotFoundResult>(result.Result);
            #endregion
        }

        [Fact]
        public async Task AddFuelCard_ReturnsOk_WhenFuelCardIsSuccessfullyAdded()
        {
            #region Arrange
            var fuelCardStoreMock = FuelCardStoreMock.GetFuelCardsStoreMock();
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            var fuelCardController = new FuelCardsController(fuelCardStoreMock.Object, fuelCardDriverStoreMock.Object);
            var fuelCardToAdd = new FuelCardDto
            {
                Id = Guid.NewGuid(),
                Pin = 1234,
                ValidityDate = DateTime.Now.AddYears(2),
                CreatedAt = DateTime.Now.AddYears(-2),
                CardNumber = "123456789",
                Status = Status.Active
            };
            #endregion

            #region Act
            var result = await fuelCardController.AddFuelCard(fuelCardToAdd);
            #endregion

            #region Assert
            Assert.IsType<OkResult>(result);
            #endregion
        }

        [Fact]
        public async Task AddFuelCard_ReturnsBadRequest_WhenFuelCardDtoIsNull()
        {
            #region Arrange
            var fuelCardStoreMock = FuelCardStoreMock.GetFuelCardsStoreMock();
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            var fuelCardController = new FuelCardsController(fuelCardStoreMock.Object, fuelCardDriverStoreMock.Object);
            #endregion

            #region Act
            var result = await fuelCardController.AddFuelCard(null);
            #endregion

            #region Assert
            Assert.IsType<BadRequestObjectResult>(result);
            #endregion
        }

        [Fact]
        public async Task UpdateFuelCard_ReturnsOk_WhenFuelCardIsSuccessfullyUpdated()
        {
            #region Arrange
            var fuelCardStoreMock = FuelCardStoreMock.GetFuelCardsStoreMock();
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            var fuelCardController = new FuelCardsController(fuelCardStoreMock.Object, fuelCardDriverStoreMock.Object);
            var fuelCardToUpdate = new FuelCardDto
            {
                Id = Guid.NewGuid(),
                Pin = 1234,
                ValidityDate = DateTime.Now.AddYears(2),
                CreatedAt = DateTime.Now.AddYears(-2),
                CardNumber = "123456789",
                Status = Status.Active
            };
            #endregion

            #region Act
            var result = await fuelCardController.UpdateFuelCard(fuelCardToUpdate);
            #endregion

            #region Assert
            Assert.IsType<OkResult>(result);
            #endregion
        }

        [Fact]
        public async Task UpdateFuelCard_ReturnsBadRequest_WhenFuelCardDtoIsNull()
        {
            #region Arrange
            var fuelCardStoreMock = FuelCardStoreMock.GetFuelCardsStoreMock();
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            var fuelCardController = new FuelCardsController(fuelCardStoreMock.Object, fuelCardDriverStoreMock.Object);
            #endregion

            #region Act
            var result = await fuelCardController.UpdateFuelCard(null);
            #endregion

            #region Assert
            Assert.IsType<BadRequestObjectResult>(result);
            #endregion
        }

        [Fact]
        public async Task DeleteFuelCard_ReturnsOk_WhenFuelCardIsSuccessfullyDeleted()
        {
            #region Arrange
            var fuelCardStoreMock = FuelCardStoreMock.GetFuelCardsStoreMock();
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            var fuelCardController = new FuelCardsController(fuelCardStoreMock.Object, fuelCardDriverStoreMock.Object);
            var fuelCardId = new Guid("a7245037-c683-4f82-b261-5c053502ed93");
            #endregion

            #region Act
            var result = await fuelCardController.DeleteFuelCard(fuelCardId);
            #endregion

            #region Assert
            Assert.IsType<OkResult>(result);
            #endregion
        }

        [Fact]
        public async Task DeleteFuelCard_ReturnsNotFound_WhenFuelCardDoesNotExist()
        {
            #region Arrange
            var fuelCardStoreMock = FuelCardStoreMock.GetFuelCardsStoreMock();
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            fuelCardStoreMock?.Setup(store => store.GetFuelCardByFuelCardIdAsync(It.IsAny<Guid>()))
                          .ReturnsAsync((FuelCard)null);
            var fuelCardController = new FuelCardsController(fuelCardStoreMock.Object, fuelCardDriverStoreMock.Object);
            #endregion

            #region Act
            var result = await fuelCardController.DeleteFuelCard(Guid.Empty);
            #endregion

            #region Assert
            Assert.IsType<NotFoundObjectResult>(result);
            #endregion
        }

        [Fact]
        public async Task GetFuelCardIncludedDriversByFuelCardId_ReturnsCorrectFuelCard()
        {
            #region Arrange
            var fuelCardStoreMock = FuelCardStoreMock.GetFuelCardsStoreMock();
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            var fuelCardController = new FuelCardsController(fuelCardStoreMock.Object, fuelCardDriverStoreMock.Object);
            var expectedFuelcardId = new Guid("a7245037-c683-4f82-b261-5c053502ed93");
            #endregion

            #region Act
            var result = await fuelCardController.GetFuelCardIncludedDriversByFuelCardId(expectedFuelcardId);
            #endregion

            #region Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedFuelCard = Assert.IsType<FuelCardDto>(actionResult.Value);
            Assert.Equal(expectedFuelcardId, returnedFuelCard.Id);
            #endregion
        }

        [Fact]
        public async Task GetFuelCardIncludedDriversByFuelCardId_ReturnsNotFound_WhenFuelCardDoesNotExist()
        {
            #region Arrange
            var fuelCardStoreMock = FuelCardStoreMock.GetFuelCardsStoreMock();
            var fuelCardDriverStoreMock = FuelCardDriverStoreMock.GetFuelCardDriverStoreMock();
            fuelCardStoreMock?.Setup(store => store.GetFuelCardByFuelCardIdAsync(It.IsAny<Guid>()))
                          .ReturnsAsync((FuelCard)null);
            var fuelCardController = new FuelCardsController(fuelCardStoreMock.Object, fuelCardDriverStoreMock.Object);
            #endregion

            #region Act
            var result = await fuelCardController.GetFuelCardIncludedDriversByFuelCardId(Guid.Empty);
            #endregion

            #region Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
            #endregion
        }
    }
}
