using AllPhi.HoGent.Blazor.Dto.Enums;

namespace AllPhi.HoGent.Blazor.ExcelModel
{
    public class VehicleExcelModel
    {
        public Guid Id { get; set; }

        public string ChassisNumber { get; set; } = string.Empty;

        public string LicensePlate { get; set; } = string.Empty;

        public string CarBrand { get; set; } = string.Empty;

        public string FuelType { get; set; } = string.Empty;

        public string TypeOfCar { get; set; } = string.Empty;

        public string Color { get; set; } = string.Empty;

        public string NumberOfDoors { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string InspectionDate { get; set; } = string.Empty;

        public string CreatedAt { get; set; } = string.Empty;
    }
}
