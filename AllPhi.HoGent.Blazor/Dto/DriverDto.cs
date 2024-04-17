using AllPhi.HoGent.Blazor.Dto.Enums;

namespace AllPhi.HoGent.Blazor.Dto
{
    public class DriverDto
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string Street { get; set; } = string.Empty;

        public string HouseNumber { get; set; } = string.Empty;

        public string PostalCode { get; set; } = string.Empty;

        public string RegisterNumber { get; set; }

        public TypeOfDriverLicense TypeOfDriverLicense { get; set; }

        public Status Status { get; set; } = Status.Active;

        public List<FuelCardDto>? FuelCardDtos { get; set; }
        public List<VehicleDto>? VehicleDtos { get; set; }
    }
}
