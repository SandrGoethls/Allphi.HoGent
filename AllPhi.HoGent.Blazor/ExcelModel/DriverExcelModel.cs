using AllPhi.HoGent.Blazor.Dto.Enums;

namespace AllPhi.HoGent.Blazor.ExcelModel
{
    public class DriverExcelModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string Street { get; set; } = string.Empty;

        public string HouseNumber { get; set; } = string.Empty;

        public string PostalCode { get; set; } = string.Empty;

        public string RegisterNumber { get; set; } = string.Empty;

        public string DateOfBirth { get; set; } = string.Empty;

        public string TypeOfDriverLicense { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string CreatedAt { get; set; } = string.Empty;
    }
}
