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
            try
            {
                if (fuelcardId == null || fuelcardId.Equals(Guid.Empty))
                {
                    return NotFound("FuelcardId is empty");
                }

                var fuelCard = await _fuelCardStore.GetFuelCardByFuelCardIdAsync(fuelcardId);

                if (fuelCard == null)
                {
                    return NotFound("No fuelcard found.");
                }

                FuelCardDto fuelCardDto = MapToFuelCardDto(fuelCard);
                return Ok(fuelCardDto);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }


        [HttpGet("getallfuelcards")]
        public async Task<ActionResult<FuelCardListDto>> GetAllFuelCards([FromQuery] string? searchByCardNumber,
                                                                         [FromQuery][Optional] string? sortBy,
                                                                         [FromQuery][Optional] bool isAscending,
                                                                         [FromQuery] int? pageNumber = null,
                                                                         [FromQuery] int? pageSize = null)
        {
            try
            {
                FilterFuelCard? filterFuelCard = new() { SearchByCardNumber = searchByCardNumber ?? "" };

                Pagination? pagination = null;
                if (pageNumber.HasValue && pageSize.HasValue)
                {
                    pagination = new Pagination(pageNumber.Value, pageSize.Value);
                }

                var (fuelCards, count) = await _fuelCardStore.GetAllFuelCardsAsync(filterFuelCard, sortBy, isAscending, pagination);

                //if (!fuelCards.Any())
                //{
                //    return NotFound( new { Message = "No vehicles found."});
                //}

                var fuelCardListDtos = new FuelCardListDto
                {
                    FuelCardDtos = MapToFuelCardListDto(fuelCards),
                    TotalItems = count
                };

                return Ok(fuelCardListDtos);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }


        [HttpPost("addfuelcard")]
        public async Task<IActionResult> AddFuelCard([FromBody] FuelCardDto fuelCardDto)
        {
            try
            {
                if (fuelCardDto == null)
                {
                    return BadRequest(new { message = "FuelCardDto cannot be null." });
                }

                FuelCard fuelCard = MapToFuelCard(fuelCardDto);
                await _fuelCardStore.AddFuelCard(fuelCard);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }


        [HttpPost("updatefuelcard")]
        public async Task<IActionResult> UpdateFuelCard([FromBody] FuelCardDto fuelCardDto)
        {
            try
            {
                if (fuelCardDto == null)
                {
                    return BadRequest(new { message = "FuelCardDto cannot be null." });
                }

                FuelCard fuelCardModel = MapToFuelCard(fuelCardDto);
                await _fuelCardStore.UpdateFuelCard(fuelCardModel);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpDelete("deletefuelcard/{fuelcardid}")]
        public async Task<IActionResult> DeleteFuelCard(Guid fuelcardid)
        {
            try
            {
                if (fuelcardid.Equals(Guid.Empty))
                {
                    return NotFound(new { Message = "Fuelcard Id cannot be null." });
                }

                await _fuelCardStore.RemoveFuelCard(fuelcardid);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }


        [HttpGet("getfuelcardincludeddriversbyfuelcardid/{fuelcardId}")]
        public async Task<ActionResult<FuelCardDto>> GetFuelCardIncludedDriversByFuelCardId(Guid fuelcardId)
        {
            try
            {
                FuelCard fuelCard = await _fuelCardStore.GetFuelCardByFuelCardIdAsync(fuelcardId);
                List<FuelCardDriver> fuelCardDriver = await _fuelCardDriverStore.GetFuelCardWithConnectedDriversByFuelCardId(fuelcardId);

                if (fuelCard == null || fuelCard.Equals(Guid.Empty))
                {
                    return NotFound(new { Message = "Fuelcard Id cannot be null." });
                }

                FuelCardDto fuelCardDto = MapToFuelCardDto(fuelCard);
                fuelCardDto.Drivers = fuelCardDriver.Select(x => x.Driver).ToList();

                return Ok(fuelCardDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }
    }
}