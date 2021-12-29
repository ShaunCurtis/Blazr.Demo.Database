/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.Auth.JWT.Simple.Core;
using System.Net.Http.Headers;
namespace Blazr.Demo.Database.Data
{
    /// <summary>
    /// This is the client version of the data broker.
    /// It's used in the Wasm SPA and gets its data from the API 
    /// </summary>
    public class WeatherForecastAPIDataBroker : IWeatherForecastDataBroker
    {
        private readonly HttpClient? httpClient;
        private readonly IClientAuthenticationService clientAuthenticationService;

        public WeatherForecastAPIDataBroker(HttpClient httpClient, IClientAuthenticationService clientAuthenticationService)
        {
            this.httpClient = httpClient!;
            this.clientAuthenticationService = clientAuthenticationService;
        }

        public async ValueTask<bool> AddForecastAsync(DcoWeatherForecast record)
        {
            this.AddJWTTokenAuthorization();
            var response = await this.httpClient!.PostAsJsonAsync<DcoWeatherForecast>($"/api/weatherforecast/add", record);
            var result = await response.Content.ReadFromJsonAsync<bool>();
            return result;
        }

        public async ValueTask<bool> UpdateForecastAsync(DcoWeatherForecast record)
        {
            this.AddJWTTokenAuthorization();
            var response = await this.httpClient!.PostAsJsonAsync<DcoWeatherForecast>($"/api/weatherforecast/update", record);
            var result = await response.Content.ReadFromJsonAsync<bool>();
            return result;
        }

        public async ValueTask<bool> DeleteForecastAsync(Guid Id)
        {
            this.AddJWTTokenAuthorization();
            var response = await this.httpClient!.PostAsJsonAsync<Guid>($"/api/weatherforecast/delete", Id);
            var result = await response.Content.ReadFromJsonAsync<bool>();
            return result;
        }

        public async ValueTask<DcoWeatherForecast> GetForecastAsync(Guid Id)
        {
            this.AddJWTTokenAuthorization();
            var response = await this.httpClient!.PostAsJsonAsync<Guid>($"/api/weatherforecast/get", Id);
            var result = await response.Content.ReadFromJsonAsync<DcoWeatherForecast>();
            return result;
        }

        public async ValueTask<List<DcoWeatherForecast>> GetWeatherForecastsAsync()
        {
            this.AddJWTTokenAuthorization();
            var list = await this.httpClient!.GetFromJsonAsync<List<DcoWeatherForecast>>($"/api/weatherforecast/list");
            return list!;
        }

        private void AddJWTTokenAuthorization()
        {
            var token = this.clientAuthenticationService.GetCurrentSessionToken();
            this.httpClient!.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.JwtToken);
        }
    }
}
