using AllPhi.HoGent.Datalake.Data.Models;
using AllPhi.HoGent.Datalake.Data.Models.Enums;

namespace AllPhi.HoGent.RestApi.Dto
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

        public string RegisterNumber { get; set; } = string.Empty;

        public TypeOfDriverLicense TypeOfDriverLicense { get; set; }

        public Status Status { get; set; } = Status.Active;

        public List<FuelCard>? FuelCards { get; set; }
    }
}
