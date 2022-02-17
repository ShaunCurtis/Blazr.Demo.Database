/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.Demo.Database.API.Web;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private IWeatherForecastDataBroker weatherForecastDataBroker;

    public WeatherForecastController(IWeatherForecastDataBroker weatherForecastDataBroker)
        => this.weatherForecastDataBroker = weatherForecastDataBroker;

    [Authorize(Policy = "IsVisitor")]
    [Route("/api/weatherforecast/list")]
    [HttpPost]
    public async Task<IActionResult> GetForecastsAsync(ListOptions options)
    {
        var list = await weatherForecastDataBroker.GetWeatherForecastsAsync(Guid.NewGuid(), options);
        return Ok(list);
    }

    [Authorize(Policy = "IsVisitor")]
    [Route("/api/weatherforecast/count")]
    [HttpGet]
    public async ValueTask<int> GetForecastCountAsync()
        => await weatherForecastDataBroker.GetWeatherForecastCountAsync(Guid.NewGuid());

    [Authorize(Policy = "IsVisitor")]
    [Route("/api/weatherforecast/get")]
    [HttpPost]
    public async Task<DcoWeatherForecast> GetForecastAsync([FromBody] Guid Id)
        => await weatherForecastDataBroker.GetForecastAsync(Guid.NewGuid(), Id);

    [Authorize(Policy = "IsUser")]
    [Route("/api/weatherforecast/add")]
    [HttpPost]
    public async Task<bool> AddRecordAsync([FromBody] DcoWeatherForecast record)
        => await weatherForecastDataBroker.AddForecastAsync(Guid.NewGuid(), record);

    [Authorize(Policy = "IsUser")]
    [Route("/api/weatherforecast/update")]
    [HttpPost]
    public async Task<bool> UpdateRecordAsync([FromBody] DcoWeatherForecast record)
        => await weatherForecastDataBroker.UpdateForecastAsync(Guid.NewGuid(), record);

    [Authorize(Policy = "IsUser")]
    [Route("/api/weatherforecast/delete")]
    [HttpPost]
    public async Task<bool> DeleteRecordAsync([FromBody] Guid Id)
        => await weatherForecastDataBroker.DeleteForecastAsync(Guid.NewGuid(), Id);
}

