using AllPhi.HoGent.Datalake.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllPhi.HoGent.Datalake.Data.Models
{
    public class FuelCardFuelType
    {
        public Guid Id { get; set; }

        public Guid FuelCardId { get; set; }

        public FuelType FuelType { get; set; }

        public FuelCard FuelCard { get; set; }
    }
}
