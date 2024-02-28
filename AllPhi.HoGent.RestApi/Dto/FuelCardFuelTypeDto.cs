using AllPhi.HoGent.Datalake.Data.Models.Enums;
using AllPhi.HoGent.Datalake.Data.Models;

namespace AllPhi.HoGent.RestApi.Dto
{
    public class FuelCardFuelTypeDto
    {
        public Guid Id { get; set; }

        public Guid FuelCardId { get; set; }

        public FuelType FuelType { get; set; }

        public FuelCard? FuelCard { get; set; }
    }
}
