using AllPhi.HoGent.Datalake.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllPhi.HoGent.Datalake.Data.Models
{
    public class Vehicle
    {
        public Guid Id { get; set; }

        public string ChassisNumber { get; set; } = string.Empty;

        public string LicensePlate { get; set; } = string.Empty;

        public CarBrand CarBrand { get; set; } = CarBrand.Bmw;

        public NumberOfDoors NumberOfDoors { get; set; } = NumberOfDoors.FiveDoors;

        public FuelType FuelType { get; set; } = FuelType.Benzine;

        public TypeOfCar TypeOfCar { get; set; } = TypeOfCar.PassangerCar;

        public VehicleColor VehicleColor { get; set; } = VehicleColor.Black;

        public Status Status { get; set; } = Status.Active;

        public DateTime InspectionDate { get; set; } 

        [NotMapped] public List<Driver>? Drivers { get; set; }
    }
}
