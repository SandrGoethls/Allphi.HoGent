using AllPhi.HoGent.Blazor.Dto.Enums;

namespace AllPhi.HoGent.Blazor.Dto
{
    public class FuelCardDto
    {
        public Guid Id { get; set; }

        public int Pin { get; set; }

        public string CardNumber { get; set; } = string.Empty;

        public DateTime ValidityDate { get; set; } = DateTime.Now;

        public List<FuelCardFuelTypeDto>? FuelCardFuelTypesDto { get; set; } = new();
        public List<DriverDto>? DriverDtos { get; set; }

        public Status Status { get; set; } = Status.Active;
    }
}
