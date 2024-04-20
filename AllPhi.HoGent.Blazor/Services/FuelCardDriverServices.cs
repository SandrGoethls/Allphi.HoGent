using AllPhi.HoGent.Blazor.Dto;
using Newtonsoft.Json;
using System.Text;

namespace AllPhi.HoGent.Blazor.Services
{
    public class FuelCardDriverServices : IFuelCardDriverServices
    {
        private readonly HttpClient _httpClient;

        public FuelCardDriverServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
    }
}
