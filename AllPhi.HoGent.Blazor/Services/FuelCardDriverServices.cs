using AllPhi.HoGent.Blazor.Dto;
using AllPhi.HoGent.Datalake.Data.Helpers;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;

namespace AllPhi.HoGent.Blazor.Services
{
    public class FuelCardDriverServices : IFuelCardDriverServices
    {
        private readonly HttpClient _httpClient;

        public FuelCardDriverServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<FuelCardDriverListDto> GetAllFuelCardDriverAsync([Optional] string sortBy, [Optional] bool isAscending, Pagination? pagination = null)
        {
            var querystring = HttpUtility.ParseQueryString(string.Empty);
            if(!string.IsNullOrEmpty(sortBy))
            {
                querystring["sortBy"] = sortBy;
                querystring["isAscending"] = isAscending.ToString();
            }
            if(pagination != null)
            {
                querystring["pageNumber"] = pagination.PageNumber.ToString();
                querystring["pageSize"] = pagination.PageSize.ToString();
            }

            string url = $"api/fuelcarddriver/getallfuelcarddrivers?{querystring}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error fetching fuelcard's drivers: {response.ReasonPhrase}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var fuelCardDriverListDto = JsonConvert.DeserializeObject<FuelCardDriverListDto>(responseContent);
            return fuelCardDriverListDto ?? new FuelCardDriverListDto();

            
        }

        public async Task<(List<FuelCardDriverDto>, bool status, string message)> GetDriverWithConnectedFuelCardsByDriverId(Guid driverId)
        {
            var response = await _httpClient.GetAsync($"api/fuelcarddriver/getdriverwithfuelcards/{driverId}");

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error fetching fuelcard's drivers: {errorResponse}");
                return (new(), false, $"Error fetching fuelcard's drivers: {response.ReasonPhrase}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var fuelCardDriverListDto = JsonConvert.DeserializeObject<List<FuelCardDriverDto>>(responseContent);
            return (fuelCardDriverListDto ?? new(), true, "Request successfully");
        }

        public async Task<(List<FuelCardDriverDto>, bool status, string message)> GetFuelCardWithConnectedDriversByFuelCardId(Guid fuelCardId)
        {
            var response = await _httpClient.GetAsync($"api/fuelcarddriver/getfuelcardwithdrivers/{fuelCardId}");

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error fetching fuelcard's drivers: {errorResponse}");
                return (new(), false, $"Error fetching fuelcard's drivers: {response.ReasonPhrase}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var fuelCardDriverListDto = JsonConvert.DeserializeObject<List<FuelCardDriverDto>>(responseContent);
            return (fuelCardDriverListDto ?? new(), true, "Request successfully");
        }


        public async Task<(bool, string message)> UpdateDriverWithFuelCards(Guid driverId, List<Guid> fuelcardGuids)
        {
            var json = JsonConvert.SerializeObject(fuelcardGuids);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/fuelcarddriver/updatedriverfuelcards/{driverId}", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error updating fuel card: {errorResponse}");
                return (false, errorResponse);
            }

            return (true, "Updated succesfully");
        }

        public async Task<(bool, string message)> UpdateFuelCardDriverAsync(Guid fuelCardId, List<Guid> driverGuids)
        {
            var json = JsonConvert.SerializeObject(driverGuids);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/fuelcarddriver/updatefuelcarddrivers/{fuelCardId}", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error updating fuel card: {errorResponse}");
                return (false, errorResponse);
            }

            return (true, "Updated succesfully");
        }
    }
}
