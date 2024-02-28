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
        [Display(Name = "1 deur")]
        OneDoor = 1,

        [Display(Name = "2 deuren")]
        TwooDoors = 2,

        [Display(Name = "3 deuren")]
        ThreeDoors = 3,

        [Display(Name = "4 deuren")]
        FourDoors = 4,

        [Display(Name = "5 deuren")]
        FiveDoors = 5,
    }
}
