using AllPhi.HoGent.Datalake.Data.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AllPhi.HoGent.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuelCardDriverController : ControllerBase
    {
        private readonly IFuelCardDriverStore _fuelCardDriverStore;

        public FuelCardDriverController(IFuelCardDriverStore fuelCardDriverStore)
        {
            _fuelCardDriverStore = fuelCardDriverStore;
        }

        // [HttpGet("getallfuelcarddrivers")]
        // [HttpGet("getdriverwithfuelcards/{driverId}")]
        // [HttpGet("getfuelcardwithdrivers/{fuelcardId}")]
        // [HttpPost("updatedriverfuelcards/{driverId}")]
        // [HttpPost("updatefuelcarddrivers/{fuelcardId}")]
    }
}
