/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.Auth.JWT.Simple.Core;
using Blazr.Core;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Blazr.Demo.Database.Data;

/// <summary>
/// This is the client version of the data broker.
/// It's used in the Wasm SPA and gets its data from the API 
/// </summary>
public class WeatherForecastAPIDataBroker : IWeatherForecastDataBroker
{
    private readonly HttpClient httpClient;
    private readonly IClientAuthenticationService clientAuthenticationService;
    private readonly ResponseMessageStore _responseMessageStore;

    public WeatherForecastAPIDataBroker(HttpClient httpClient, IClientAuthenticationService clientAuthenticationService, ResponseMessageStore responseMessageStore)
    {
        this.httpClient = httpClient!;
        this.clientAuthenticationService = clientAuthenticationService;
        this._responseMessageStore = responseMessageStore;
    }

    public async ValueTask<bool> AddForecastAsync(Guid transactionId, DcoWeatherForecast record)
    {
        var result = false;
        this.AddJWTTokenAuthorization();
        var response = await this.httpClient.PostAsJsonAsync<DcoWeatherForecast>($"/api/weatherforecast/add", record);
        this.AddMessageToMessageStore(transactionId, response);
        if (response.IsSuccessStatusCode)
            result = await response.Content.ReadFromJsonAsync<bool>();
        return result;
    }

    public async ValueTask<bool> UpdateForecastAsync(Guid transactionId, DcoWeatherForecast record)
    {
        var result = false;
        this.AddJWTTokenAuthorization();
        var response = await this.httpClient.PostAsJsonAsync<DcoWeatherForecast>($"/api/weatherforecast/update", record);
        this.AddMessageToMessageStore(transactionId, response);
        if (response.IsSuccessStatusCode)
            result = await response.Content.ReadFromJsonAsync<bool>();
        return result;
    }

    public async ValueTask<bool> DeleteForecastAsync(Guid transactionId, Guid Id)
    {
        var result = false;
        this.AddJWTTokenAuthorization();
        var response = await this.httpClient.PostAsJsonAsync<Guid>($"/api/weatherforecast/delete", Id);
        this.AddMessageToMessageStore(transactionId, response);
        if (response.IsSuccessStatusCode)
            result = await response.Content.ReadFromJsonAsync<bool>();
        return result;
    }

    public async ValueTask<DcoWeatherForecast> GetForecastAsync(Guid transactionId, Guid Id)
    {
        this.AddJWTTokenAuthorization();
        var record = new DcoWeatherForecast();
        var response = await this.httpClient.PostAsJsonAsync<Guid>($"/api/weatherforecast/get", Id);
        this.AddMessageToMessageStore(transactionId, response);
        if (response.IsSuccessStatusCode)
            record = await response.Content.ReadFromJsonAsync<DcoWeatherForecast>();
        return record ?? new DcoWeatherForecast();
    }

    public async ValueTask<List<DvoWeatherForecast>> GetWeatherForecastsAsync(Guid transactionId, ListOptions options)
    {
        var list = new List<DvoWeatherForecast>();

        this.AddJWTTokenAuthorization();
        var response = await this.httpClient.PostAsJsonAsync<ListOptions>($"/api/weatherforecast/list", options) ;
        this.AddMessageToMessageStore(transactionId, response);
        if (response.IsSuccessStatusCode)
        {
            list = await response.Content.ReadFromJsonAsync<List<DvoWeatherForecast>>();
        }
        return list ?? new();
    }

    public async ValueTask<int> GetWeatherForecastCountAsync(Guid transactionId)
    {
        var ret = 0;
        this.AddJWTTokenAuthorization();
        var response = await this.httpClient.GetAsync($"/api/weatherforecast/listcount");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
             ret = JsonSerializer.Deserialize<int>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        return ret;
    }

    private void AddJWTTokenAuthorization()
    {
        var token = this.clientAuthenticationService.GetCurrentSessionToken();
        this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.JwtToken);
    }

    private void AddMessageToMessageStore(Guid transactionId, HttpResponseMessage response)
        => _responseMessageStore.AddMessage(new ResponseMessage
        {
            Id = transactionId,
            OK = response.IsSuccessStatusCode,
            Message = $"Server returned {response.StatusCode}"
        });
}


