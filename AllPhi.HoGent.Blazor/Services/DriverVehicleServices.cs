using AllPhi.HoGent.Blazor.Dto;
using AllPhi.HoGent.Datalake.Data.Helpers;
using AllPhi.HoGent.Datalake.Data.Models;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;

namespace AllPhi.HoGent.Blazor.Services
{
    public class DriverVehicleServices : IDriverVehicleServices
    {
        private readonly HttpClient _httpClient;

        public DriverVehicleServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(List<DriverVehicleDto>, bool status, string message)> GetDriverWithConnectedVehiclesByDriverId(Guid driverId)
        {
            var response = await _httpClient.GetAsync($"api/drivervehicle/getdriverwithvehiclesbydriverid/{driverId}");

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error fetching driver's vehicles: {errorResponse}");
                return (new(), false, $"Error fetching driver's vehicles: {response.ReasonPhrase}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var driverVehicleDtos = JsonConvert.DeserializeObject<List<DriverVehicleDto>>(responseContent);
            return (driverVehicleDtos ?? new(), true, "Request successfully");
        }

        public async Task<(List<DriverVehicleDto>, bool status, string message)> GetVehicleWithConnectedDriversByVehicleId(Guid vehicleId)
        {
            var response = await _httpClient.GetAsync($"api/drivervehicle/getvehiclewithdrivers/{vehicleId}");

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error fetching vehicles's drivers: {errorResponse}");
                return (new(), false, $"Error fetching vehicles's drivers: {response.ReasonPhrase}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var driverVehicleDtos = JsonConvert.DeserializeObject<List<DriverVehicleDto>>(responseContent);
            return (driverVehicleDtos ?? new(), true, "Request successfully");
        }

        public async Task<(bool, string message)> UpdateVehicleWithDrivers(Guid vehicleId, List<Guid> driverGuids)
        {
            var json = JsonConvert.SerializeObject(driverGuids);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/drivervehicle/updatevehicledriversbyvehicleid/{vehicleId}", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error updating vehicle: {errorResponse}");
                return (false, errorResponse);
            }

            return (true, "Updated succesfully");
        }

        public async Task<(bool, string message)> UpdateDriverWithVehicles(Guid driverId, List<Guid> vehicleGuids)
        {
            var json = JsonConvert.SerializeObject(vehicleGuids);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/drivervehicle/updatedrivervehiclesbydriverid/{driverId}", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error updating driver: {errorResponse}");
                return (false, errorResponse);
            }

            return (true, "Updated succesfully");
        }

        public async Task<DriverVehicleListDto> GetAllDriverVehicleAsync([Optional] string sortBy, [Optional] bool isAscending, Pagination? pagination = null)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            if (!string.IsNullOrEmpty(sortBy)) queryString["sortBy"] = sortBy;
            queryString["isAscending"] = isAscending.ToString();
            if (pagination != null)
            {
                queryString["pageNumber"] = pagination.PageNumber.ToString();
                queryString["pageSize"] = pagination.PageSize.ToString();
            }

            string url = $"api/drivervehicle/getalldrivervehicles?{queryString}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error fetching driver vehicles: {response.ReasonPhrase}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var driverVehiclesDto = JsonConvert.DeserializeObject<DriverVehicleListDto>(responseContent);
            return driverVehiclesDto ?? new DriverVehicleListDto();

            
        }
    }
}
