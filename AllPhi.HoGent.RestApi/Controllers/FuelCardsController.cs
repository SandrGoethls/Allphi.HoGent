using AllPhi.HoGent.Datalake.Data.Helpers;
using AllPhi.HoGent.Datalake.Data.Models;
using AllPhi.HoGent.Datalake.Data.Store;
using AllPhi.HoGent.RestApi.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Runtime.InteropServices;
using static AllPhi.HoGent.RestApi.Extensions.FuelCardMapperExtension;

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
        public async Task<ActionResult<FuelCardDto>> GetFuelCardById(Guid fuelcardId)
        {
            var fuelCard = await _fuelCardStore.GetFuelCardByFuelCardIdAsync(fuelcardId);
            if (fuelCard == null)
            {
                return NotFound();
            }
            FuelCardDto fuelCardDto = MapToFuelCardDto(fuelCard);
            return Ok(fuelCardDto);
        }

        [HttpGet("getallfuelcards")]
        public async Task<ActionResult<FuelCardListDto>> GetAllFuelCards([Optional] string? sortby, [Optional] bool isAcending, Pagination? pagination)
        {
            var (fuelCards, count) = await _fuelCardStore.GetAllFuelCardsAsync(sortby, isAcending, pagination);
            if (fuelCards == null)
            {
                return NotFound();
            }
            var fuelCardListDtos = new FuelCardListDto
            {
                FuelCardDtos = MapToFuelCardListDto(fuelCards),
                TotalItems = count
            };
            return Ok(fuelCardListDtos);
        }

        [HttpPost("addfuelcard")]
        public async Task<IActionResult> AddFuelCard([FromBody]FuelCardDto fuelCardDto)
        {
            FuelCard fuelCard = MapToFuelCard(fuelCardDto);
            await _fuelCardStore.AddFuelCard(fuelCard);
            return Ok();
        }

        [HttpPost("updatefuelcard")]
        public async Task<IActionResult> UpdateFuelCard(FuelCardDto fuelCard)
        {
            FuelCard fuelCardModel = MapToFuelCard(fuelCard);
            await _fuelCardStore.UpdateFuelCard(fuelCardModel);
            return Ok();
        }

        [HttpDelete("deletefuelcard/{fuelcardid}")]
        public async Task<IActionResult> DeleteFuelCard(Guid fuelcardid)
        {
            await _fuelCardStore.RemoveFuelCard(fuelcardid);
            return Ok();
        }

        [HttpGet("getfuelcardincludeddriversbyfuelcardid/{fuelcardId}")]
        public async Task<ActionResult<FuelCardDto>> GetFuelCardIncludedDriversByFuelCardId(Guid fuelcardId)
        {
            FuelCard fuelCard = await _fuelCardStore.GetFuelCardByFuelCardIdAsync(fuelcardId);
            List<FuelCardDriver> fuelCardDriver = await _fuelCardDriverStore.GetFuelCardWithConnectedDriversByFuelCardId(fuelcardId);

            if (fuelCard == null)
            {
                return NotFound();
            }

            FuelCardDto fuelCardDto = MapToFuelCardDto(fuelCard);
            fuelCardDto.Drivers = fuelCardDriver.Select(x => x.Driver).ToList();
            return Ok(fuelCardDto);
        }
    }
}