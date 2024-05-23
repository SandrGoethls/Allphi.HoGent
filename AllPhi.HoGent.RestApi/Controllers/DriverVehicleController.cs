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
            try
            {
                var (driverVehicles, count) = await _driverVehicleStore.GetAllDriverVehicleAsync();

                if (!driverVehicles.Any())
                {
                    return NotFound(new { Messagd = "No drivers found." });
                }

                var driverVehicleListDtos = new DriverVehicleListDto
                {
                    DriverVehicleDtos = MapToDriverVehicleListDto(driverVehicles),
                    TotalItems = count
                };
                return Ok(driverVehicleListDtos);
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Invalid driver data: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Detail = ex.Message });
            }
        }

        [HttpGet("getdriverwithvehiclesbydriverid/{driverId}")]
        public async Task<ActionResult<DriverVehicleListDto>> GetDriverWithVehiclesByDriverId(Guid driverId)
        {
            try
            {
                if (driverId.Equals(Guid.Empty))
                {
                    return BadRequest(new { Message = "DriverId cannot be empty." });
                }

                var driverVehicles = await _driverVehicleStore.GetDriverWithConnectedVehicleByDriverId(driverId);

                if (!driverVehicles.Any())
                {
                    return NotFound(new { Message = "No vehicles to be found." });
                }

                return Ok(MapToDriverVehicleListDto(driverVehicles));
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Invalid driver ID: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Detail = ex.Message });
            }
        }

        [HttpGet("getvehiclewithdrivers/{vehicleId}")]
        public async Task<ActionResult<DriverVehicleListDto>> GetVehicleWithDriversByVehicleId(Guid vehicleId)
        {
            try
            {
                if (vehicleId.Equals(Guid.Empty))
                {
                    return BadRequest(new { Message = "Vehicle ID not found." });
                }

                var driverVehicles = await _driverVehicleStore.GetVehicleWithConnectedDriversByVehicleId(vehicleId);

                if (!driverVehicles.Any())
                {
                    return NotFound(new { Message = "No drivers found for this vehicle ID." });
                }

                var driverVehicleDtos = MapToDriverVehicleListDto(driverVehicles);
                return Ok(driverVehicleDtos);
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Invalid driver ID: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Detail = ex.Message });
            }
        }


        [HttpPost("updatedrivervehiclesbydriverid/{driverId}")]
        public async Task<IActionResult> UpdateDriverVehiclesByDriverId(Guid driverId, [FromBody] List<Guid> newVehicleIds)
        {
            try
            {
                if (driverId.Equals(Guid.Empty))
                {
                    return BadRequest(new { Message = "Driver ID cannot be empty." });
                }

                if (newVehicleIds == null || !newVehicleIds.Any())
                {
                    return BadRequest(new { Message = "Vehicle IDs list cannot be null or empty." });
                }

                await _driverVehicleStore.UpdateDriverWithVehiclesByDriverIdAndListOfVehicleIds(driverId, newVehicleIds);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Invalid driver ID: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Detail = ex.Message });
            }
        }


        [HttpPost("updatevehicledriversbyvehicleid/{vehicleId}")]
        public async Task<IActionResult> UpdateVehicleDriversByVehicleId(Guid vehicleId, [FromBody] List<Guid> newDriverIds)
        {
            try
            {
                if (vehicleId.Equals(Guid.Empty))
                {
                    return BadRequest(new { Message = "Vehicle ID cannot be empty." });
                }

                if (!newDriverIds.Any())
                {
                    return BadRequest(new { Message = "Driver IDs list cannot be null or empty." });
                }

                await _driverVehicleStore.UpdateVehicleWithDriversByFuelCardIdAndDriverIds(vehicleId, newDriverIds);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Invalid driver ID: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Detail = ex.Message });
            }
        }

    }
}