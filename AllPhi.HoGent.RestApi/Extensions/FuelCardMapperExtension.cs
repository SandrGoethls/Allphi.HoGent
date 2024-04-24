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
                    FuelCardId = f.FuelCardId,
                    Id = f.Id
                }).ToList(),
                Drivers = fuelCard.Drivers,
                Status = fuelCard.Status
            };
        }

        
        public static List<FuelCardDto> MapToFuelCardListDto(List<FuelCard> fuelCards)
        {
            return fuelCards.Select(f => new FuelCardDto
            {
                Id = f.Id,
                Pin = f.Pin,
                CardNumber = f.CardNumber,
                ValidityDate = f.ValidityDate,
                FuelCardFuelTypesDto = f.FuelCardFuelTypes.Select(f => new FuelCardFuelTypeDto
                {
                    FuelType = f.FuelType,
                    FuelCardId = f.FuelCardId,
                    Id = f.Id
                }).ToList(),
                Drivers = f.Drivers,
                Status = f.Status
            }).ToList();

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
