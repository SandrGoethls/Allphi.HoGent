using AllPhi.HoGent.Datalake.Data.Models;
using AllPhi.HoGent.RestApi.Dto;
using Microsoft.AspNetCore.Mvc;

namespace AllPhi.HoGent.RestApi.Extensions
{
    public  static class FuelCardMapperExtension
    {
        
        public static FuelCardDto MapToFuelCardDto(FuelCard fuelCard)
        {
            return new FuelCardDto
            {
                Id = fuelCard.Id,
                Pin = fuelCard.Pin,
                CardNumber = fuelCard.CardNumber,
                ValidityDate = fuelCard.ValidityDate,
                FuelCardFuelTypesDto = fuelCard.FuelCardFuelTypes.Select(f => new FuelCardFuelTypeDto
                {
                    FuelType = f.FuelType,
                    FuelCardId = f.FuelCardId
                }).ToList(),
                Drivers = fuelCard.Drivers,
                Status = fuelCard.Status
            };
        }

        
        public static FuelCardListDto MapToFuelCardListDto(List<FuelCard> fuelCards, int count)
        {
            return new FuelCardListDto
            {
                FuelCards = fuelCards.Select(MapToFuelCardDto).ToList(),
                TotalItems = count
            };

        }

        public static FuelCard MapToFuelCard(FuelCardDto fuelCardDto)
        {
            return new FuelCard
            {
                Id = fuelCardDto.Id,
                Pin = fuelCardDto.Pin,
                CardNumber = fuelCardDto.CardNumber,
                ValidityDate = fuelCardDto.ValidityDate,
                FuelCardFuelTypes = fuelCardDto.FuelCardFuelTypesDto.Select(f => new FuelCardFuelType
                {
                    FuelType = f.FuelType,
                    FuelCardId = f.FuelCardId
                }).ToList(),
                Drivers = fuelCardDto.Drivers,
                Status = fuelCardDto.Status
            };
        }
    }
}
