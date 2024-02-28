using AllPhi.HoGent.Datalake.Data.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AllPhi.HoGent.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleStore _vehicleStore;
        private readonly IDriverVehicleStore _driverVehicleStore;

        public VehiclesController(IVehicleStore vehicleStore)
        {
            _vehicleStore = vehicleStore;
        }

        // [HttpGet("getvehiclebyid/{vehicleId}")]
        // [HttpGet("getvehicleincludeddriversbydriverid/{vehicleId}")]
        // [HttpGet("getallvehicles")]
        // [HttpPost("addvehicle")]
        // [HttpPost("updatevehicle")]
        // [HttpDelete("deletevehicle/{vehicleid}")]
    }
}
