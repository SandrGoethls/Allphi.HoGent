using System.ComponentModel.DataAnnotations.Schema;

namespace AllPhi.HoGent.Blazor.Dto
{
    public class DriverVehicleDto
    {
        public Guid DriverId { get; set; }
        [NotMapped] public DriverDto DriverDto { get; set; }

        public Guid VehicleId { get; set; }
        [NotMapped] public VehicleDto VehicleDto { get; set; }
    }
}
