using AllPhi.HoGent.Datalake.Data.Helpers;
using AllPhi.HoGent.Datalake.Data.Models;
using AllPhi.HoGent.Datalake.Data.Store;
using AllPhi.HoGent.RestApi.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Runtime.InteropServices;
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
            try
            {
                if (vehicleId == null || vehicleId.Equals(Guid.Empty))
                {
                    return NotFound(new { message = "VehicleId is empty." });
                }

                Vehicle vehicle = await _vehicleStore.GetVehicleByIdAsync(vehicleId);

                if (vehicle == null)
                {
                    return NotFound(new { message = "Vehicle not found." });
                }

                VehicleDto vehicleDto = MapToVehicleDto(vehicle);
                return Ok(vehicleDto);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }


        [HttpGet("getallvehicles")]
        public async Task<ActionResult<VehicleListDto>> GetAllVehicles([FromQuery] string? searchByLicencePlate,
                                                                       [FromQuery] string? searchByChassisNumber,
                                                                       [FromQuery][Optional] string? sortBy,
                                                                       [FromQuery][Optional] bool isAscending,
                                                                       [FromQuery] int? pageNumber = null,
                                                                       [FromQuery] int? pageSize = null)
        {
            try
            {

                FilterVehicle? filterVehicle = new() { SearchByLicencePlate = searchByLicencePlate ?? "", SearchByChassisNumber = searchByChassisNumber ?? "" };

                Pagination? pagination = null;
                if (pageNumber.HasValue && pageSize.HasValue)
                {
                    pagination = new Pagination(pageNumber.Value, pageSize.Value);
                }

                var (vehicles, count) = await _vehicleStore.GetAllVehiclesAsync(filterVehicle, sortBy, isAscending, pagination);

                if (!vehicles.Any())
                {
                    return NotFound(new { Message = "No vehicles found." });
                }

                var vehicleListDto = new VehicleListDto
                {
                    VehicleDtos = MapToVehicleListDto(vehicles),
                    TotalItems = count
                };

                return Ok(vehicleListDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpPost("addvehicle")]
        public async Task<IActionResult> AddVehicle(VehicleDto vehicleDto)
        {
            try
            {
                if (vehicleDto == null)
                {
                    return BadRequest("No vehicle data provided.");
                }

                if (_vehicleStore.VehicleWithChassisNumberExists(vehicleDto.ChassisNumber))
                {
                    return BadRequest("Vehicle with this chassis number already exists");
                }
                if (_vehicleStore.VehicleWithLicensePlateExists(vehicleDto.LicensePlate))
                {
                    return BadRequest("Vehicle with this license plate already exists");
                }

                Vehicle vehicle = MapToVehicle(vehicleDto);
                await _vehicleStore.AddVehicle(vehicle);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Invalid argument: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status409Conflict, $"Operation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred: {ex.Message}");
            }
        }


        [HttpPost("updatevehicle")]
        public async Task<IActionResult> UpdateVehicle(VehicleDto vehicleDto)
        {
            try
            {
                if (vehicleDto == null)
                {
                    return BadRequest(new { Message = "No vehicle found." });
                }

                Vehicle vehicle = MapToVehicle(vehicleDto);
                await _vehicleStore.UpdateVehicle(vehicle);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Invalid argument: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound($"Operation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred: {ex.Message}");
            }
        }


        [HttpDelete("deletevehicle/{vehicleid}")]
        public async Task<IActionResult> DeleteVehicle(Guid vehicleId)
        {
            try
            {
                if (vehicleId == null || vehicleId.Equals(Guid.Empty))
                {
                    return NotFound(new { message = "Vehicle ID is empty." });
                }

                await _vehicleStore.RemoveVehicle(vehicleId);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Invalid argument: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound($"Operation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred: {ex.Message}");
            }
        }

        [HttpGet("getvehiclebydriverid/{driverId}")]
        public async Task<ActionResult<VehicleListDto>> GetVehicleByDriverId(Guid driverId)
        {
            try
            {
                if (driverId == null || driverId.Equals(Guid.Empty))
                {
                    return BadRequest(new { Message = "No Driver ID Found." });
                }

                List<DriverVehicle> vehicles = await _driverVehicleStore.GetDriverWithConnectedVehicleByDriverId(driverId);
                if (!vehicles.Any())
                {
                    return NotFound($"No vehicles found for driver ID: {driverId}");
                }

                VehicleListDto vehicleListDto = new VehicleListDto();
                foreach (var vehicle in vehicles)
                {
                    VehicleDto vehicleDto = MapToVehicleDto(vehicle.Vehicle);
                    vehicleListDto.VehicleDtos.Add(vehicleDto);
                }

                return Ok(vehicleListDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Invalid argument: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound($"Operation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred: {ex.Message}");
            }
        }

    }
}