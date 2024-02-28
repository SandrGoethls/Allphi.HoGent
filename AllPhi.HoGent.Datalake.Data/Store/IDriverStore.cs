using AllPhi.HoGent.Datalake.Data.Helpers;
using AllPhi.HoGent.Datalake.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AllPhi.HoGent.Datalake.Data.Store
{
    public interface IDriverStore
    {
        Task<Driver> GetDriverByIdAsync(Guid driverId);
        Task<(List<Driver>, int)> GetAllDriversAsync([Optional] string sortBy, [Optional] bool isAscending, Pagination? pagination = null);
        Task AddDriver(Driver driver);
        Task UpdateDriver(Driver driver);
        Task RemoveDriver(Guid driverId);
    }
}
