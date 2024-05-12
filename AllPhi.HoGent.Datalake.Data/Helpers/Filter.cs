using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllPhi.HoGent.Datalake.Data.Helpers
{
    public class Filter
    {

    }

    public class FilterVehicle
    {
        public string? SearchByLicencePlate { get; set; } = string.Empty;
        public string? SearchByChassisNumber { get; set; } = string.Empty;
    }

    public class FilterFuelCard
    {
        public string SearchByCardNumber { get; set; } = string.Empty;
    }
}
