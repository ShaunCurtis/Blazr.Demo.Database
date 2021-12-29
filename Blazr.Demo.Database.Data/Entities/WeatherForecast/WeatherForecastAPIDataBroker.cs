/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.Auth.JWT.Simple.Core;
using Blazr.Core;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Blazr.Demo.Database.Data
{
    /// <summary>
    /// This is the client version of the data broker.
    /// It's used in the Wasm SPA and gets its data from the API 
    /// </summary>
    public class WeatherForecastAPIDataBroker : IWeatherForecastDataBroker
    {
        private readonly HttpClient? _httpClient;
        private HttpClient httpClient => _httpClient!;
        private readonly IClientAuthenticationService clientAuthenticationService;
        private readonly ResponseMessageStore _responseMessageStore;

        public WeatherForecastAPIDataBroker(HttpClient httpClient, IClientAuthenticationService clientAuthenticationService, ResponseMessageStore responseMessageStore)
        {
            this._httpClient = httpClient!;
            this.clientAuthenticationService = clientAuthenticationService;
            this._responseMessageStore = responseMessageStore;
        }

        public async ValueTask<bool> AddForecastAsync(Guid transactionId, DcoWeatherForecast record)
        {
            this.AddJWTTokenAuthorization();
            var response = await this.httpClient.PostAsJsonAsync<DcoWeatherForecast>($"/api/weatherforecast/add", record);
            var result = await response.Content.ReadFromJsonAsync<bool>();
            return result;
        }

        public async ValueTask<bool> UpdateForecastAsync(Guid transactionId, DcoWeatherForecast record)
        {
            this.AddJWTTokenAuthorization();
            var response = await this.httpClient.PostAsJsonAsync<DcoWeatherForecast>($"/api/weatherforecast/update", record);
            var result = await response.Content.ReadFromJsonAsync<bool>();
            return result;
        }

        public async ValueTask<bool> DeleteForecastAsync(Guid transactionId, Guid Id)
        {
            this.AddJWTTokenAuthorization();
            var response = await this.httpClient.PostAsJsonAsync<Guid>($"/api/weatherforecast/delete", Id);
            var result = await response.Content.ReadFromJsonAsync<bool>();
            return result;
        }

        public async ValueTask<DcoWeatherForecast> GetForecastAsync(Guid transactionId, Guid Id)
        {
            this.AddJWTTokenAuthorization();
            var response = await this.httpClient.PostAsJsonAsync<Guid>($"/api/weatherforecast/get", Id);
            var result = await response.Content.ReadFromJsonAsync<DcoWeatherForecast>();
            return result;
        }

        public async ValueTask<List<DcoWeatherForecast>> GetWeatherForecastsAsync(Guid transactionId)
        {
            List<DcoWeatherForecast>? list = new();

            this.AddJWTTokenAuthorization();
            // list = await this.httpClient.GetFromJsonAsync<List<DcoWeatherForecast>>($"/api/weatherforecast/list");
            var response = await this.httpClient.GetAsync($"/api/weatherforecast/list");
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                list = JsonSerializer.Deserialize<List<DcoWeatherForecast>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
            _responseMessageStore.AddMessage(new ResponseMessage
            {
                Id = transactionId,
                OK = response.IsSuccessStatusCode,
                Message = $"Server returned {response.StatusCode}"
            });
            return list!;
        }

        private void AddJWTTokenAuthorization()
        {
            var token = this.clientAuthenticationService.GetCurrentSessionToken();
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.JwtToken);
        }
    }
}
