using AllPhi.HoGent.Datalake.Data.Models;
using AllPhi.HoGent.Datalake.Data.Models.Enums;

namespace AllPhi.HoGent.RestApi.Dto
{
    public class VehicleDto
    {
        public Guid Id { get; set; }

        public string ChassisNumber { get; set; } = string.Empty;

        public string LicensePlate { get; set; } = string.Empty;

        public CarBrand CarBrand { get; set; }

        public FuelType FuelType { get; set; }

        public TypeOfCar TypeOfCar { get; set; }

        public VehicleColor Color { get; set; }

        public NumberOfDoors NumberOfDoors { get; set; }

        public Status Status { get; set; } = Status.Active;

        public DateTime InspectionDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<Driver>? Drivers { get; set; }
    }
}
