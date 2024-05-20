using AllPhi.HoGent.Datalake.Data.Helpers;
using AllPhi.HoGent.Datalake.Data.Models;
using AllPhi.HoGent.Datalake.Data.Store;
using AllPhi.HoGent.RestApi.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Runtime.InteropServices;
using static AllPhi.HoGent.RestApi.Extensions.FuelCardDriverMapperExtension;

namespace AllPhi.HoGent.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("AllPhiFixedLimiter")]
    public class FuelCardDriverController : ControllerBase
    {
        private readonly IFuelCardDriverStore _fuelCardDriverStore;

        public FuelCardDriverController(IFuelCardDriverStore fuelCardDriverStore)
        {
            _fuelCardDriverStore = fuelCardDriverStore;
        }

        [HttpGet("getallfuelcarddrivers")]
        public async Task<ActionResult<FuelCardDriverListDto>> GetAllFuelCardDrivers([Optional] string? sortBy, [Optional] bool isAscending, Pagination? pagination = null)
        {
            try
            {
                var (fuelCardDrivers, count) = await _fuelCardDriverStore.GetAllFuelCardDriverAsync(sortBy, isAscending, pagination);

                if (fuelCardDrivers == null)
                {
                    return NotFound(new { Message = "No fuel card drivers found." });
                }

                var fuelCardDriverListDto = new FuelCardDriverListDto
                {
                    FuelCardDriverDtos = MapToFuelCardDriverListDto(fuelCardDrivers),
                    TotalItems = count
                };

                return Ok(fuelCardDriverListDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Detail = ex.Message });
            }
        }

        [HttpGet("getdriverwithfuelcards/{driverId}")]
        public async Task<IActionResult> GetDriverWithFuelCardsByDriverId(Guid driverId)
        {
            try
            {
                if (driverId.Equals(Guid.Empty))
                {
                    return NotFound(new { Message = "DriverId cannot be empty." });
                }

                var fuelCardDrivers = await _fuelCardDriverStore.GetDriverWithConnectedFuelCardsByDriverId(driverId);

                if (!fuelCardDrivers.Any())
                {
                    return NotFound(new { Message = "No fuel cards found for this driver ID." });
                }

                return Ok(MapToFuelCardDriverListDto(fuelCardDrivers));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getfuelcardwithdrivers/{fuelcardId}")]
        public async Task<IActionResult> GetFuelCardWithDriversByFuelCardId(Guid fuelcardId)
        {
            try
            {
                if (fuelcardId.Equals(Guid.Empty))
                {
                    return NotFound(new { Message = "Fuelcard ID cannot be empty." });
                }

                var fuelCardDrivers = await _fuelCardDriverStore.GetFuelCardWithConnectedDriversByFuelCardId(fuelcardId);

                if (fuelCardDrivers == null || !fuelCardDrivers.Any())
                {
                    return NotFound(new { Message = "No drivers found for this fuel card ID." });
                }

                return Ok(MapToFuelCardDriverListDto(fuelCardDrivers));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Detail = ex.Message });
            }
        }


        [HttpPost("updatedriverfuelcards/{driverId}")]
        public async Task<IActionResult> UpdateDriverFuelCardsByDriverId(Guid driverId, [FromBody] List<Guid> newFuelCardIds)
        {
            try
            {
                if (driverId.Equals(Guid.Empty))
                {
                    return NotFound(new { Message = "No driver ID can be found." });
                }

                if (newFuelCardIds == null)
                {
                    return BadRequest(new { Message = "No fuel cards found." });
                }

                await _fuelCardDriverStore.UpdateDriverWithFuelCardsByDriverIdAndListOfFuelCardIds(driverId, newFuelCardIds);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Detail = ex.Message });
            }
        }

        [HttpPost("updatefuelcarddrivers/{fuelcardId}")]
        public async Task<IActionResult> UpdateFuelCardDriversByFuelCardId(Guid fuelcardId, [FromBody] List<Guid> newDriverIds)
        {
            try
            {
                if (fuelcardId.Equals(Guid.Empty))
                {
                    return NotFound(new { Message = "Fuelcard ID cannot be empty." });
                }

                if (newDriverIds == null)
                {
                    return BadRequest(new { Message = "Parameter newDriverIds was null." });
                }

                await _fuelCardDriverStore.UpdateFuelCardWithDriversByFuelCardIdAndDriverIds(fuelcardId, newDriverIds);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Detail = ex.Message });
            }
        }
    }
}