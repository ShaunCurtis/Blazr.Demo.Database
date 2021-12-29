﻿
using Microsoft.AspNetCore.Authorization;
/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.Demo.Database.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private IWeatherForecastDataBroker weatherForecastDataBroker;

        public WeatherForecastController(IWeatherForecastDataBroker weatherForecastDataBroker)
            => this.weatherForecastDataBroker = weatherForecastDataBroker;

        [Authorize(Policy = "IsVisitor")]
        [Route("/api/weatherforecast/list")]
        [HttpGet]
        public async Task<List<DcoWeatherForecast>> GetForecastsAsync()
            => await weatherForecastDataBroker.GetWeatherForecastsAsync();

        [Authorize(Policy = "IsVisitor")]
        [Route("/api/weatherforecast/get")]
        [HttpPost]
        public async Task<DcoWeatherForecast> GetForecastAsync([FromBody] Guid Id)
            => await weatherForecastDataBroker.GetForecastAsync(Id);

        [Authorize(Policy = "IsUser")]
        [Route("/api/weatherforecast/add")]
        [HttpPost]
        public async Task<bool> AddRecordAsync([FromBody] DcoWeatherForecast record)
            => await weatherForecastDataBroker.AddForecastAsync(record);

        [Authorize(Policy = "IsUser")]
        [Route("/api/weatherforecast/update")]
        [HttpPost]
        public async Task<bool> UpdateRecordAsync([FromBody] DcoWeatherForecast record)
            => await weatherForecastDataBroker.UpdateForecastAsync(record);

        [Authorize(Policy = "IsUser")]
        [Route("/api/weatherforecast/delete")]
        [HttpPost]
        public async Task<bool> DeleteRecordAsync([FromBody] Guid Id)
            => await weatherForecastDataBroker.DeleteForecastAsync(Id);
    }
}
