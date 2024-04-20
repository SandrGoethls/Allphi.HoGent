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
    }
}
