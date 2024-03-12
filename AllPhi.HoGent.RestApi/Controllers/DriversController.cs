using AllPhi.HoGent.Datalake.Data.Models;
using AllPhi.HoGent.Datalake.Data.Store;
using AllPhi.HoGent.RestApi.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using AllPhi.HoGent.RestApi.Extensions;
using static AllPhi.HoGent.RestApi.Extensions.DriverMapperExtensions;

namespace AllPhi.HoGent.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("AllPhiFixedLimiter")]
    public class DriversController : ControllerBase
    {
        private readonly IDriverStore _driverStore;
        private readonly IFuelCardDriverStore _fuelCardDriverStore;

        public DriversController(IDriverStore driverStore, IFuelCardDriverStore fuelCardDriverStore)
        {
            _driverStore = driverStore;
            _fuelCardDriverStore = fuelCardDriverStore;
        }

        [HttpGet("getalldrivers")]
        public async Task<IActionResult> GetAllDrivers()
        {
            var (drivers, count) = await _driverStore.GetAllDriversAsync();
            if (drivers == null)
            {
                return NotFound();
            }
            List<DriverListDto> driverListDtos = new List<DriverListDto>();
            driverListDtos.Add(MapToDriverListDto(drivers, count));
            return Ok(driverListDtos);
        }

        [HttpGet("getdriverbyid/{driverId}")]
        public async Task<IActionResult> GetDriverById(Guid driverId)
        {
            Driver driver = await _driverStore.GetDriverByIdAsync(driverId);
            if (driver == null)
            {
                return NotFound();
            }
            var driverDto = MapToDriverDto(driver);
            return Ok(driverDto);
        }

        [HttpPost("adddriver")]
        public async Task<IActionResult> AddDriver([FromBody]DriverDto driverDto)
        {
            Driver driver = MapToDriver(driverDto);
            await _driverStore.AddDriver(driver);
            return Ok();
        }

        [HttpPost("updatedriver")]
        public async Task<IActionResult> UpdateDriver([FromBody]DriverDto driverDto)
        {
            Driver driver = MapToDriver(driverDto);
            await _driverStore.UpdateDriver(driver);
            return Ok();
        }

        [HttpDelete("deletedriver/{driverId}")]
        public async Task<IActionResult> DeleteDriver(Guid driverId)
        {
            await _driverStore.RemoveDriver(driverId);
            return Ok();
        }

        [HttpGet("getdriverincludedfuelcardsbydriverid/{driverId}")]
        public async Task<IActionResult> GetDriverIncludedFuelCardsByDriverId(Guid driverId)
        {
            var driverWithFuelCards = await _fuelCardDriverStore.GetDriverWithConnectedFuelCardsByDriverId(driverId);
            Driver driver = await _driverStore.GetDriverByIdAsync(driverId);
            if (driverWithFuelCards == null)
            {
                return NotFound();
            }
            var driverDto = MapToDriverDto(driver);
            driverDto.FuelCards = driverWithFuelCards.Select(x => x.FuelCard).ToList();
            return Ok(driverDto);
        }
    }
}