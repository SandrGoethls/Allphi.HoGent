using System.ComponentModel.DataAnnotations.Schema;

namespace AllPhi.HoGent.Blazor.Dto
{
    public class FuelCardDriverDto
    {
        public Guid DriverId { get; set; }
        [NotMapped] public DriverDto DriverDto { get; set; }


        public Guid FuelCardId { get; set; }
        [NotMapped] public FuelCardDto FuelCardDto { get; set; }
    }
}
