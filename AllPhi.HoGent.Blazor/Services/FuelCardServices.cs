using AllPhi.HoGent.Blazor.Dto;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;

namespace AllPhi.HoGent.Blazor.Services
{
    public class FuelCardServices : IFuelCardServices
    {
        private readonly HttpClient _httpClient;

        public FuelCardServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<FuelCardListDto> GetAllFuelCardsAsync([Optional] string? sortBy, [Optional] bool isAscending, [Optional] int pageNumber, [Optional] int pageSize)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            if (!string.IsNullOrEmpty(sortBy)) queryString["sortBy"] = sortBy;
            queryString["isAscending"] = isAscending.ToString();
            queryString["pageNumber"] = pageNumber.ToString();
            queryString["pageSize"] = pageSize.ToString();

            string url = $"api/fuelcards/getallfuelcards?{queryString}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error fetching fuel cards: {response.ReasonPhrase}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var fuelCardsDto = JsonConvert.DeserializeObject<FuelCardListDto>(responseContent);
            return fuelCardsDto ?? new FuelCardListDto();
        }

        public async Task<(bool, string message)> AddFuelCardAsync(FuelCardDto fuelCardDto)
        {
            try
            {
                var json = JsonConvert.SerializeObject(fuelCardDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/fuelcards/addfuelcard", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error adding fuel card: {errorResponse}");
                    return (false, errorResponse);
                }

                return (true, "Added succesfully");
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> DeleteFuelCardAsync(Guid fuelCardId)
        {
            var response = await _httpClient.DeleteAsync($"api/fuelcards/deletefuelcard/{fuelCardId}");

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error deleting fuel card: {errorResponse}");
                return false;
            }

            return true;
        }
    }
}
