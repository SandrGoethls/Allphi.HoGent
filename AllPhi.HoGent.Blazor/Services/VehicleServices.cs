using AllPhi.HoGent.Blazor.Dto;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;

namespace AllPhi.HoGent.Blazor.Services
{
    public class VehicleServices : IVehicleServices
    {
        private readonly HttpClient _httpClient;

        public VehicleServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<VehicleListDto> GetAllVehicleAsync([Optional] string? sortBy, [Optional] bool isAscending, [Optional] int pageNumber, [Optional] int pageSize)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            if (!string.IsNullOrEmpty(sortBy)) queryString["sortBy"] = sortBy;
            queryString["isAscending"] = isAscending.ToString();
            queryString["pageNumber"] = pageNumber.ToString();
            queryString["pageSize"] = pageSize.ToString();

            string url = $"api/vehicles/getallvehicles?{queryString}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error fetching Vehicles: {response.ReasonPhrase}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var vehiclesDto = JsonConvert.DeserializeObject<VehicleListDto>(responseContent);
            return vehiclesDto ?? new VehicleListDto();
        }

        public async Task<VehicleDto> GetVehicleByIdAsync(Guid vehicleId)
        {
            var response = await _httpClient.GetAsync($"api/vehicles/getvehiclebyid/{vehicleId}");

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error fetching fuel card: {response.ReasonPhrase}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var vehicelDto = JsonConvert.DeserializeObject<VehicleDto>(responseContent);
            return vehicelDto ?? new();
        }

        public async Task<(bool, string message)> AddFVehicleAsync(VehicleDto vehicleDto)
        {
            try
            {
                var json = JsonConvert.SerializeObject(vehicleDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/vehicles/addvehicle", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error adding vehicle: {errorResponse}");
                    return (false, errorResponse);
                }

                return (true, "Added succesfully");
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> DeleteVehicleAsync(Guid vehicleId)
        {
            var response = await _httpClient.DeleteAsync($"api/vehicles/deletevehicle/{vehicleId}");

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error deleting vehicle: {errorResponse}");
                return false;
            }

            return true;
        }

        public async Task<bool> UpdateVehicleAsync(VehicleDto vehicleDto)
        {
            var json = JsonConvert.SerializeObject(vehicleDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/vehicles/updatevehicle", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error updating vehicle: {errorResponse}");
                return false;
            }

            return true;
        }
    }
}
