using AllPhi.HoGent.Blazor.Dto;
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

        public async Task<DriverListDto> GetAllDriversAsync([Optional] string? sortBy, [Optional] bool isAscending, [Optional] int pageNumber, [Optional] int pageSize)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            if (!string.IsNullOrEmpty(sortBy)) queryString["sortBy"] = sortBy;
            queryString["isAscending"] = isAscending.ToString();
            queryString["pageNumber"] = pageNumber.ToString();
            queryString["pageSize"] = pageSize.ToString();

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
    }
}
