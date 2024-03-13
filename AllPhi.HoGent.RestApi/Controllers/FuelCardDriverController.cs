using AllPhi.HoGent.Datalake.Data.Helpers;
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
    public class FuelCardDriverController : ControllerBase
    {
        private readonly IFuelCardDriverStore _fuelCardDriverStore;

        public FuelCardDriverController(IFuelCardDriverStore fuelCardDriverStore)
        {
            _fuelCardDriverStore = fuelCardDriverStore;
        }

        [HttpGet("getallfuelcarddrivers")]
        public async Task<ActionResult<FuelCardDriverListDto>> GetAllFuelCardDrivers([FromQuery] string sortBy, [FromQuery] bool isAscending, [FromQuery] Pagination pagination)
        {
            var (fuelCardDrivers, count) = await _fuelCardDriverStore.GetAllFuelCardDriverAsync(sortBy, isAscending, pagination);
            if (fuelCardDrivers == null)
            {
                return NotFound();
            }
            var fuelCardDriverListDto = new FuelCardDriverListDto();
            fuelCardDriverListDto.Equals(MapToFuelCardDriverListDto(fuelCardDrivers));

            return Ok(fuelCardDriverListDto);
        }

        [HttpGet("getdriverwithfuelcards/{driverId}")]
        public async Task<IActionResult> GetDriverWithFuelCardsByDriverId(Guid driverId)
        {
            var fuelCardDrivers = await _fuelCardDriverStore.GetDriverWithConnectedFuelCardsByDriverId(driverId);
            return Ok(MapToFuelCardDriverListDto(fuelCardDrivers));
        }

        [HttpGet("getfuelcardwithdrivers/{fuelcardId}")]
        public async Task<IActionResult> GetFuelCardWithDriversByFuelCardId(Guid fuelcardId)
        {
            var fuelCardDrivers = await _fuelCardDriverStore.GetFuelCardWithConnectedDriversByFuelCardId(fuelcardId);
            return Ok(MapToFuelCardDriverListDto(fuelCardDrivers));
        }

        [HttpPost("updatedriverfuelcards/{driverId}")]
        public async Task<IActionResult> UpdateDriverFuelCardsByDriverId(Guid driverId, [FromBody] List<Guid> newFuelCardIds)
        {
            await _fuelCardDriverStore.UpdateDriverWithFuelCardsByDriverIdAndListOfFuelCardIds(driverId, newFuelCardIds);
            return Ok();
        }

        [HttpPost("updatefuelcarddrivers/{fuelcardId}")]
        public async Task<IActionResult> UpdateFuelCardDriversByFuelCardId(Guid fuelcardId, [FromBody] List<Guid> newDriverIds)
        {
            await _fuelCardDriverStore.UpdateFuelCardWithDriversByFuelCardIdAndDriverIds(fuelcardId, newDriverIds);
            return Ok();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public FuelCardDriverDto MapToFuelCardDriverDto(FuelCardDriver fuelCardDriver)
        {
            return new FuelCardDriverDto
            {
                DriverId = fuelCardDriver.DriverId,
                FuelCardId = fuelCardDriver.FuelCardId
            };
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public FuelCardDriverListDto MapToFuelCardDriverListDto(List<FuelCardDriver> fuelCardDrivers)
        {
            return new FuelCardDriverListDto
            {
                FuelCardDriverDtos = fuelCardDrivers.Select(MapToFuelCardDriverDto).ToList(),
                TotalItems = fuelCardDrivers.Count
            };
        }
    }
}