using AllPhi.HoGent.Datalake.Data.Models;
using AllPhi.HoGent.Datalake.Data.Store;
using AllPhi.HoGent.RestApi.Dto;
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

        [HttpGet("getvehiclebyid/{vehicleId}")]
        public async Task<IActionResult> GetVehicleById(Guid vehicleId)
        {
            Vehicle vehicle = await _vehicleStore.GetVehicleByIdAsync(vehicleId);
            if (vehicle == null)
            {
                return NotFound();
            }
            VehicleDto vehicleDto = MapToVehicleDto(vehicle);
            return Ok(vehicleDto);
        }

        // [HttpGet("getvehicleincludeddriversbydriverid/{vehicleId}")]
        [HttpGet("getallvehicles")]
        public async Task<IActionResult> GetAllVehicles()
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
        public async Task<IActionResult> AddVehicle(Vehicle vehicle)
        {
            await _vehicleStore.AddVehicle(vehicle);
            return Ok();
        }

        [HttpPost("updatevehicle")]
        public async Task<IActionResult> UpdateVehicle(Vehicle vehicle)
        {
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
        public async Task<IActionResult> GetVehicleByDriverId(Guid driverId)
        {
            var vehicles = await _driverVehicleStore.GetDriverWithConnectedVehicleByDriverId(driverId);
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

        [ApiExplorerSettings(IgnoreApi = true)]
        private VehicleDto MapToVehicleDto(Vehicle vehicle)
        {
            return new VehicleDto
            {
                Id = vehicle.Id,
                ChassisNumber = vehicle.ChassisNumber,
                LicensePlate = vehicle.LicensePlate,
                CarBrand = vehicle.CarBrand,
                FuelType = vehicle.FuelType,
                TypeOfCar = vehicle.TypeOfCar,
                Color = vehicle.VehicleColor,
                NumberOfDoors = vehicle.NumberOfDoors,
                Status = vehicle.Status
            };
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private VehicleListDto MapToVehicleListDto(List<Vehicle> vehicles, int count)
        {
            return new VehicleListDto
            {
                VehicleDtos = vehicles.Select(MapToVehicleDto).ToList(),
                TotalItems = count
            };
        }
    }
}