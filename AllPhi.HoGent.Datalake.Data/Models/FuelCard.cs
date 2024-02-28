using AllPhi.HoGent.Datalake.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllPhi.HoGent.Datalake.Data.Models
{
    public class FuelCard
    {
        public Guid Id { get; set; }

        public int Pin { get; set; }

        public string CardNumber { get; set; } = string.Empty;

        public DateTime ValidityDate { get; set; }

        public Status Status { get; set; } = Status.Active;

        public List<FuelCardFuelType>? FuelCardFuelTypes { get; set; } = new();

        [NotMapped] public List<Driver>? Drivers { get; set; }
    }
}
