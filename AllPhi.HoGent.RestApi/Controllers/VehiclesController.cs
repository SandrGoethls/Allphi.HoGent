using AllPhi.HoGent.Datalake.Data.Models;
using AllPhi.HoGent.Datalake.Data.Store;
using AllPhi.HoGent.RestApi.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using static AllPhi.HoGent.RestApi.Extensions.VehicleMapperExtension;


namespace AllPhi.HoGent.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("AllPhiFixedLimiter")]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleStore _vehicleStore;
        private readonly IDriverVehicleStore _driverVehicleStore;

        public VehiclesController(IVehicleStore vehicleStore, IDriverVehicleStore driverVehicleStore)
        {
            _vehicleStore = vehicleStore;
            _driverVehicleStore = driverVehicleStore;
        }

        [HttpGet("getvehiclebyid/{vehicleId}")]
        public async Task<ActionResult<VehicleDto>> GetVehicleById(Guid vehicleId)
        {
            Vehicle vehicle = await _vehicleStore.GetVehicleByIdAsync(vehicleId);
            if (vehicle == null)
            {
                return NotFound();
            }
            VehicleDto vehicleDto = MapToVehicleDto(vehicle);
            return Ok(vehicleDto);
        }

        
        [HttpGet("getallvehicles")]
        public async Task<ActionResult<VehicleListDto>> GetAllVehicles()
        {
            var (vehicles, count) = await _vehicleStore.GetAllVehiclesAsync();
            if (vehicles == null)
            {
                return NotFound();
            }
            List<VehicleListDto> vehicleListDtos = new List<VehicleListDto>();
            vehicleListDtos.Add(MapToVehicleListDto(vehicles, count));
            return Ok(vehicleListDtos);
        }

        [HttpPost("addvehicle")]
        public async Task<IActionResult> AddVehicle(VehicleDto vehicleDto)
        {
            Vehicle vehicle = MapToVehicle(vehicleDto);
            await _vehicleStore.AddVehicle(vehicle);
            return Ok();
        }

        [HttpPost("updatevehicle")]
        public async Task<IActionResult> UpdateVehicle(VehicleDto vehicleDto)
        {
            Vehicle vehicle = MapToVehicle(vehicleDto);
            await _vehicleStore.UpdateVehicle(vehicle);
            return Ok();
        }

        [HttpDelete("deletevehicle/{vehicleid}")]
        public async Task<IActionResult> DeleteVehicle(Guid vehicleId)
        {
            await _vehicleStore.RemoveVehicle(vehicleId);
            return Ok();
        }

        [HttpGet("getvehiclebydriverid/{driverId}")]
        public async Task<ActionResult<VehicleListDto>> GetVehicleByDriverId(Guid driverId)
        {
            List<DriverVehicle> vehicles = await _driverVehicleStore.GetDriverWithConnectedVehicleByDriverId(driverId);
            if (vehicles == null)
            {
                return NotFound();
            }
            VehicleListDto vehicleListDto = new VehicleListDto();
            foreach (var vehicle in vehicles)
            {
                VehicleDto vehicleDto = MapToVehicleDto(vehicle.Vehicle);
                vehicleListDto.VehicleDtos.Add(vehicleDto);
            }

            return Ok(vehicleListDto);
        }

        //[HttpGet("getvehicleincludeddriversbydriverid/{vehicleId}")]
        
    }
}