using AllPhi.HoGent.Datalake.Data.Helpers;
using AllPhi.HoGent.Datalake.Data.Models;
using AllPhi.HoGent.Datalake.Data.Store;
using AllPhi.HoGent.RestApi.Dto;
using AllPhi.HoGent.RestApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Runtime.InteropServices;
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
        public async Task<ActionResult<(List<DriverDto>, int)>> GetAllDrivers([FromQuery][Optional] string? sortBy, [FromQuery][Optional] bool isAscending, [FromQuery] int? pageNumber = null, [FromQuery] int? pageSize = null)
        {
            Pagination? pagination = null;
            if (pageNumber.HasValue && pageSize.HasValue)
            {
                pagination = new Pagination(pageNumber.Value, pageSize.Value);
            }

            var (drivers, count) = await _driverStore.GetAllDriversAsync(sortBy, isAscending, pagination);
            if (count <= 0)
            {
                return NotFound();
            }

            var driverListDtos =  new DriverListDto
            {
                DriverDtos = MapToDriverListDto(drivers),
                TotalItems = count
            };



            return Ok(driverListDtos);
        }

        [HttpGet("getdriverbyid/{driverId}")]
        public async Task<ActionResult<DriverDto>> GetDriverById(Guid driverId)
        {
            Driver driver = await _driverStore.GetDriverByIdAsync(driverId);
            if (driver == null)
            {
                return NotFound("Driver not found");
            }
            var driverDto = MapToDriverDto(driver);
            return Ok(driverDto);
        }

        [HttpPost("adddriver")]
        public async Task<IActionResult> AddDriver([FromBody] DriverDto driverDto)
        {
            if (_driverStore.DriverWithRegisterNumberExists(driverDto.RegisterNumber))
            {
                return BadRequest("Driver with this register number already exists");
            }

            if (!IsValidDriverRegisterNumberCheck.IsValidDriverRegisterNumber(driverDto.RegisterNumber, driverDto.DateOfBirth))
            {
                return BadRequest("The provided register number is not valid.");
            }
            try
            {
                Driver driver = MapToDriver(driverDto);
                await _driverStore.AddDriver(driver);
                return Ok("Driver successfully added");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("updatedriver")]
        public async Task<IActionResult> UpdateDriver([FromBody] DriverDto driverDto)
        {
            try
            {
                Driver driver = MapToDriver(driverDto);
                await _driverStore.UpdateDriver(driver);
                return Ok("Driver successfully updated!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("deletedriver/{driverId}")]
        public async Task<IActionResult> DeleteDriver(Guid driverId)
        {
            try
            {
                await _driverStore.RemoveDriver(driverId);
                return Ok($"Driver with ID {driverId} successfully deleted.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getdriverincludedfuelcardsbydriverid/{driverId}")]
        public async Task<ActionResult<DriverDto>> GetDriverIncludedFuelCardsByDriverId(Guid driverId)
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