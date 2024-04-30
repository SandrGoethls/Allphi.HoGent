using AllPhi.HoGent.Datalake.Data.Models;
using AllPhi.HoGent.Datalake.Data.Store;
using AllPhi.HoGent.RestApi.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using static AllPhi.HoGent.RestApi.Extensions.DriverVehicleMapperExtension;

namespace AllPhi.HoGent.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("AllPhiFixedLimiter")]
    public class DriverVehicleController : ControllerBase
    {
        private readonly IDriverVehicleStore _driverVehicleStore;

        public DriverVehicleController(IDriverVehicleStore driverVehicleStore)
        {
            _driverVehicleStore = driverVehicleStore;
        }

        [HttpGet("getalldrivervehicles")]
        public async Task<ActionResult<DriverVehicleListDto>> GetAllDriverVehicles()
        {
            var (driverVehicles, count) = await _driverVehicleStore.GetAllDriverVehicleAsync();
            if (driverVehicles == null)
            {
                return NotFound();
            }
            
            var driverVehicleListDtos = new DriverVehicleListDto
            {
                DriverVehicleDtos = MapToDriverVehicleListDto(driverVehicles),
                TotalItems = count
            };
            return Ok(driverVehicleListDtos);
        }

        //[HttpGet("getdriverwithvehiclesbydriverid/{driverId}")]
        //public async Task<ActionResult<DriverVehicleListDto>> GetDriverWithVehiclesByDriverId(Guid driverId)
        //{
        //    var driverVehicles = await _driverVehicleStore.GetDriverWithConnectedVehicleByDriverId(driverId);
        //    if (driverVehicles == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(MapToDriverVehicleListDto(driverVehicles, driverVehicles.Count));
        //}

        [HttpGet("getvehiclewithdrivers/{vehicleId}")]
        public async Task<ActionResult<DriverVehicleListDto>> GetVehicleWithDriversByVehicleId(Guid vehicleId)
        {
            var driverVehicles = await _driverVehicleStore.GetVehicleWithConnectedDriversByVehicleId(vehicleId);
            if (driverVehicles == null)
            {
                return NotFound();
            }

            var driverVehicleDtos = MapToDriverVehicleListDto(driverVehicles);
            return Ok(driverVehicleDtos);
        }

        [HttpPost("updatedrivervehiclesbydriverid/{driverId}")]
        public async Task<IActionResult> UpdateDriverVehiclesByDriverId(Guid driverId, [FromBody] List<Guid> newVehicleIds)
        {
            await _driverVehicleStore.UpdateDriverWithVehiclesByDriverIdAndListOfVehicleIds(driverId, newVehicleIds);
            return Ok();
        }

        [HttpPost("updatevehicledriversbyvehicleid/{vehicleId}")]
        public async Task<IActionResult> UpdateVehicleDriversByVehicleId(Guid vehicleId, [FromBody] List<Guid> newDriverIds)
        {
            await _driverVehicleStore.UpdateVehicleWithDriversByFuelCardIdAndDriverIds(vehicleId, newDriverIds);
            return Ok();
        }
    }
}