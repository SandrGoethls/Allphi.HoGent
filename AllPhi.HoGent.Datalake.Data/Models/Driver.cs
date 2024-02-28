using AllPhi.HoGent.Datalake.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllPhi.HoGent.Datalake.Data.Models
{
    public class Driver
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string Street { get; set; } = string.Empty;

        public string HouseNumber { get; set; } = string.Empty;

        public string PostalCode { get; set; } = string.Empty;

        public int RegisterNumber { get; set; }

        public TypeOfDriverLicense TypeOfDriverLicense { get; set; } = TypeOfDriverLicense.B;

        public Status Status { get; set; } = Status.Active;

        [NotMapped] public List<FuelCard>? FuelCards { get; set; }

        [NotMapped] public List<Vehicle>? Vehicles { get; set; }
    }
}
