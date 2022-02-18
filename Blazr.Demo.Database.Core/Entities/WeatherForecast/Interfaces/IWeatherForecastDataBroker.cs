/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Demo.Database.Core
{
    /// <summary>
    /// The data broker interface abstracts the interface between the logic layer and the data layer.
    /// </summary>
    public interface IWeatherForecastDataBroker
    {
        public ValueTask<bool> AddForecastAsync(Guid transactionId, DcoWeatherForecast record);

        public ValueTask<bool> UpdateForecastAsync(Guid transactionId, DcoWeatherForecast record);

        public ValueTask<DcoWeatherForecast> GetForecastAsync(Guid transactionId, Guid Id);

        public ValueTask<bool> DeleteForecastAsync(Guid transactionId, Guid Id);

        public ValueTask<List<DvoWeatherForecast>> GetWeatherForecastsAsync(Guid transactionId, ListOptions options);

        public ValueTask<int> GetWeatherForecastCountAsync(Guid transactionId);
    }
}
