using AllPhi.HoGent.Datalake.Data.Models.Enums;
using AllPhi.HoGent.Datalake.Data.Models;

namespace AllPhi.HoGent.RestApi.Dto
{
    public class FuelCardDto
    {
        public Guid Id { get; set; }

        public int Pin { get; set; }

        public string CardNumber { get; set; } = string.Empty;

        public DateTime ValidityDate { get; set; }

        public List<FuelCardFuelTypeDto>? FuelCardFuelTypesDto { get; set; } = new();
        public List<Driver>? Drivers { get; set; }

        public Status Status { get; set; } = Status.Active;
    }
}
