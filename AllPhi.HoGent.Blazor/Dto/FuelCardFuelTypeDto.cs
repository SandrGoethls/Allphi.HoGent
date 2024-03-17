using AllPhi.HoGent.Blazor.Dto.Enums;

namespace AllPhi.HoGent.Blazor.Dto
{
    public class FuelCardFuelTypeDto
    {
        public Guid Id { get; set; }

        public Guid FuelCardId { get; set; }

        public FuelType FuelType { get; set; }

        public FuelCardDto? FuelCardDto { get; set; }
    }
}
