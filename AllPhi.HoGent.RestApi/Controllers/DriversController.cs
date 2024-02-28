using AllPhi.HoGent.Datalake.Data.Store;
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

        // [HttpGet("getdriverbyid/{driverId}")]
        // [HttpGet("getdriverincludedfuelcardsbydriverid/{driverId}")]
        // [HttpGet("getalldrivers")]
        // [HttpPost("adddriver")]
        // [HttpPost("updatedriver")]
        // [HttpDelete("deletedriver/{driverId}")]
    }
}
