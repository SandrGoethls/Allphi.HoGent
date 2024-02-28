using AllPhi.HoGent.Datalake.Data.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AllPhi.HoGent.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuelCardsController : ControllerBase
    {
        private readonly IFuelCardStore _fuelCardStore;
        private readonly IFuelCardDriverStore _fuelCardDriverStore;

        public FuelCardsController(IFuelCardStore fuelCardStore, IFuelCardDriverStore fuelCardDriverStore)
        {
            _fuelCardStore = fuelCardStore;
            _fuelCardDriverStore = fuelCardDriverStore;
        }

        // [HttpGet("getfuelcardbyid/{fuelcardId}")]
        // [HttpGet("getfuelcardincludeddriversbyfuelcardid/{fuelcardId}")]
        // [HttpGet("getallfuelcards")]
        // [HttpPost("addfuelcard")]
        // [HttpPost("updatefuelcard")]
        // [HttpDelete("deletefuelcard/{fuelcardid}")]
    }
}
