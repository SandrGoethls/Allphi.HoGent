using AllPhi.HoGent.Datalake.Data.Models.Enums;
using AllPhi.HoGent.Datalake.Data.Models;
using AllPhi.HoGent.Datalake.Data.Store;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AllPhi.HoGent.Datalake.Data.Helpers;

namespace AllPhi.HoGent.Testing.MockData
{
    public static class VehicleStoreMock
    {
        public static Mock<IVehicleStore> GetVehicleStoreMock()
        {
            var mock = new Mock<IVehicleStore>();

            var mockVehicle_1 = new Vehicle
            {               
                Id = new Guid("a7245037-c683-4f82-b261-5c053502ed93"),
                ChassisNumber = "123456789",
                LicensePlate = "1-ABC-123",
                CarBrand = CarBrand.Bmw,
                NumberOfDoors = NumberOfDoors.FiveDoors,
                FuelType = FuelType.Benzine,
                TypeOfCar = TypeOfCar.PassangerCar,
                VehicleColor = VehicleColor.Black,
                Status = Status.Active,
                InspectionDate = DateTime.Now.AddYears(-1),
                CreatedAt = DateTime.Now.AddYears(-1)
            };

            var mockVehicle_2 = new Vehicle
            {
                Id = Guid.NewGuid(),
                ChassisNumber = "987654321",
                LicensePlate = "2-DEF-456",
                CarBrand = CarBrand.Audi,
                NumberOfDoors = NumberOfDoors.TwoDoors,
                FuelType = FuelType.Hybride,
                TypeOfCar = TypeOfCar.Van,
                VehicleColor = VehicleColor.Blue,
                Status = Status.Active,
                InspectionDate = DateTime.Now.AddYears(-1),
                CreatedAt = DateTime.Now.AddYears(-2)
            };

            return mock;
        }
    }
}
