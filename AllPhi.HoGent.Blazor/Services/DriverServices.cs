using AllPhi.HoGent.Blazor.Dto;
using AllPhi.HoGent.Datalake.Data.Helpers;
using Newtonsoft.Json;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;

namespace AllPhi.HoGent.Blazor.Services
{
    public class DriverServices : IDriverServices
    {
        private readonly HttpClient _httpClient;

        public DriverServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DriverListDto> GetAllDriversAsync([Optional] string? sortBy, [Optional] bool isAscending, [Optional] FilterDriver filterDriver, [Optional] Pagination pagination)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            if (!string.IsNullOrEmpty(sortBy)) 
                queryString["sortBy"] = sortBy;

            queryString["isAscending"] = isAscending.ToString();

            if (filterDriver != null)
            {
                queryString["searchByFirstName"] = filterDriver?.SearchByFirstName?.ToString();
                queryString["searchByLastName"] = filterDriver?.SearchByLastName?.ToString();
                queryString["searchByRegisternumber"] = filterDriver?.SearchByRegisternumber?.ToString();
            }

            if (pagination != null)
            {
                queryString["pageNumber"] = pagination.PageNumber.ToString();
                queryString["pageSize"] = pagination.PageSize.ToString();
            }

            string url = $"api/drivers/getalldrivers?{queryString}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error fetching drivers: {response.ReasonPhrase}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var driversDto = JsonConvert.DeserializeObject<DriverListDto>(responseContent);
            return driversDto ?? new DriverListDto();
        }

        public async Task<(bool, string message)> AddFDriverAsync(DriverDto driverDto)
        {
            try
            {
                var json = JsonConvert.SerializeObject(driverDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/drivers/adddriver", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error adding driver: {errorResponse}");
                    return (false, errorResponse);
                }

                return (true, "Added succesfully");
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> DeleteDriverAsync(Guid driverId)
        {
            var response = await _httpClient.DeleteAsync($"api/drivers/deletedriver/{driverId}");

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error deleting driver: {errorResponse}");
                return (false);
            }

            return true;
        }

        public async Task<bool> UpdateDriverAsync(DriverDto driverDto)
        {
            var json = JsonConvert.SerializeObject(driverDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/drivers/updatedriver", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error updating driver: {errorResponse}");
                return false;
            }

            return true;
        }
    }
}
