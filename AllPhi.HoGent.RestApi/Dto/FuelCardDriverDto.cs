using AllPhi.HoGent.Datalake.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace AllPhi.HoGent.RestApi.Dto
{
    public class FuelCardDriverDto
    {
        public Guid DriverId { get; set; }
        [NotMapped] public Driver Driver { get; set; }


        public Guid FuelCardId { get; set; }
        [NotMapped] public FuelCard FuelCard { get; set; }
    }
}
