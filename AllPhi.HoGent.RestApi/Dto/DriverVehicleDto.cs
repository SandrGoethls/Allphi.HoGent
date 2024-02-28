using AllPhi.HoGent.Datalake.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace AllPhi.HoGent.RestApi.Dto
{
    public class DriverVehicleDto
    {
        public Guid DriverId { get; set; }
        [NotMapped] public Driver Driver { get; set; }

        public Guid VehicleId { get; set; }
        [NotMapped] public Vehicle Vehicle { get; set; }
    }
}
