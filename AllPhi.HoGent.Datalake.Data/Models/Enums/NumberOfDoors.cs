using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllPhi.HoGent.Datalake.Data.Models.Enums
{
    public enum NumberOfDoors
    {
        [Display(Name = "2 deuren")]
        TwoDoors = 2,

        [Display(Name = "3 deuren")]
        ThreeDoors = 3,

        [Display(Name = "4 deuren")]
        FourDoors = 4,

        [Display(Name = "5 deuren")]
        FiveDoors = 5,
    }
}
