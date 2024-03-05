namespace AllPhi.HoGent.Blazor.Services
{
    public class DriverVehicleServices : IDriverVehicleServices
    {
        private readonly HttpClient _httpClient;

        public DriverVehicleServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

    }
}
