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

        [HttpGet("getdriverbyid/{driverId}")]
        public async Task<IActionResult> GetDriverById(Guid driverId)
        {
            Driver driver = await _driverStore.GetDriverByIdAsync(driverId);
            if (driver == null)
            {
                return NotFound();
            }
            DriverDto driverDto = MapToDto(driver);
            return Ok(driverDto);
        }

        private DriverDto MapToDto(Driver driver)
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
        //[HttpGet("getdriverincludedfuelcardsbydriverid/{driverId}")]
        // [HttpGet("getalldrivers")]
        // [HttpPost("adddriver")]
        // [HttpPost("updatedriver")]
        // [HttpDelete("deletedriver/{driverId}")]
    }
}
