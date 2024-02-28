using AllPhi.HoGent.Datalake.Data.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AllPhi.HoGent.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverVehicleController : ControllerBase
    {
        private readonly IDriverVehicleStore _driverVehicleStore;

        public DriverVehicleController(IDriverVehicleStore driverVehicleStore)
        {
            _driverVehicleStore = driverVehicleStore;
        }

        // [HttpGet("getalldrivervehicles")]
        // [HttpGet("getdriverwithvehiclesbydriverid/{driverId}")]
        // [HttpGet("getvehiclewithdrivers/{vehicleId}")]
        // [HttpPost("updatedrivervehiclesbydriverid/{driverId}")]
        // [HttpPost("updatevehicledriversbyvehicleid/{vehicleId}")]
    }
}
