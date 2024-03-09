using AllPhi.HoGent.Datalake.Data.Models;
using AllPhi.HoGent.Datalake.Data.Store;
using AllPhi.HoGent.RestApi.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace AllPhi.HoGent.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("AllPhiFixedLimiter")]
    public class FuelCardsController : ControllerBase
    {
        private readonly IFuelCardStore _fuelCardStore;
        private readonly IFuelCardDriverStore _fuelCardDriverStore;

        public FuelCardsController(IFuelCardStore fuelCardStore, IFuelCardDriverStore fuelCardDriverStore)
        {
            _fuelCardStore = fuelCardStore;
            _fuelCardDriverStore = fuelCardDriverStore;
        }

        [HttpGet("getfuelcardbyid/{fuelcardId}")]
        public async Task<IActionResult> GetFuelCardById(Guid fuelcardId)
        {
            var fuelCard = await _fuelCardStore.GetFuelCardByFuelCardIdAsync(fuelcardId);
            if (fuelCard == null)
            {
                return NotFound();
            }
            return Ok(fuelCard);
        }

        [HttpGet("getallfuelcards")]
        public async Task<IActionResult> GetAllFuelCards()
        {
            var (fuelCards, count) = await _fuelCardStore.GetAllFuelCardsAsync();
            if (fuelCards == null)
            {
                return NotFound();
            }
            List<FuelCardListDto> fuelCardListDtos = new List<FuelCardListDto>();
            fuelCardListDtos.Add(MapToFuelCardListDto(fuelCards, count));
            return Ok(fuelCardListDtos);
        }

        [HttpPost("addfuelcard")]
        public async Task<IActionResult> AddFuelCard(FuelCard fuelCard)
        {
            await _fuelCardStore.AddFuelCard(fuelCard);
            return Ok();
        }

        [HttpPost("updatefuelcard")]
        public async Task<IActionResult> UpdateFuelCard(FuelCard fuelCard)
        {
            await _fuelCardStore.UpdateFuelCard(fuelCard);
            return Ok();
        }

        [HttpDelete("deletefuelcard/{fuelcardid}")]
        public async Task<IActionResult> DeleteFuelCard(Guid fuelcardid)
        {
            await _fuelCardStore.RemoveFuelCard(fuelcardid);
            return Ok();
        }

        // [HttpGet("getfuelcardincludeddriversbyfuelcardid/{fuelcardId}")]


        [ApiExplorerSettings(IgnoreApi = true)]
        public FuelCardDto MapToDto(FuelCard fuelCard)
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

        [ApiExplorerSettings(IgnoreApi = true)]
        private FuelCardListDto MapToFuelCardListDto(List<FuelCard> fuelCards, int count)
        {
            return new FuelCardListDto
            {
                FuelCards = fuelCards.Select(MapToDto).ToList(),
                TotalItems = count
            };
            
        }
    }
}