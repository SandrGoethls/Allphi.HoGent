using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllPhi.HoGent.Datalake.Data.Models
{
    public class FuelCardDriver
    {
        public Guid DriverId { get; set; }
        public Driver Driver { get; set; }


        public Guid FuelCardId { get; set; }
        public FuelCard FuelCard { get; set; }

    }
}
