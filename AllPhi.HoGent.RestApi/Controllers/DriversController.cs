using AllPhi.HoGent.Datalake.Data.Models;
using AllPhi.HoGent.Datalake.Data.Store;
using AllPhi.HoGent.RestApi.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AllPhi.HoGent.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            DriverDto driverDto = MapToDriverDto(driver);
            return Ok(driverDto);
        }

        [HttpPost("adddriver")]
        public async Task<IActionResult> AddDriver(Driver driver)
        {
            await _driverStore.AddDriver(driver);
            return Ok();
        }

        [HttpPost("updatedriver")]
        public async Task<IActionResult> UpdateDriver(Driver driver)
        {
            await _driverStore.UpdateDriver(driver);
            return Ok();
        }

        [HttpDelete("deletedriver/{driverId}")]
        public async Task<IActionResult> DeleteDriver(Guid driverId)
        {
            await _driverStore.RemoveDriver(driverId);
            return Ok();
        }

        // [HttpGet("getdriverincludedfuelcardsbydriverid/{driverId}")]

        [ApiExplorerSettings(IgnoreApi = true)]
        private DriverDto MapToDriverDto(Driver driver)
        {
            return new DriverDto
            {
                Id = driver.Id,
                FirstName = driver.FirstName,
                LastName = driver.LastName,
                City = driver.City,
                Street = driver.Street,
                HouseNumber = driver.HouseNumber,
                PostalCode = driver.PostalCode,
                RegisterNumber = driver.RegisterNumber,
                TypeOfDriverLicense = driver.TypeOfDriverLicense,
                Status = driver.Status
            };
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private DriverListDto MapToDriverListDto(List<Driver> drivers, int count)
        {
            return new DriverListDto
            {
                DriverDtos = drivers.Select(MapToDriverDto).ToList(),
                TotalItems = count
            };
        }
    }
}