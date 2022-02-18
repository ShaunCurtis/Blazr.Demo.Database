/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================


namespace Blazr.Template.Tests.ViewServices
{
    public partial class WeatherForecastViewServiceTests
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(55, 55)]
        [InlineData(233, 233)]
        public async void WeatherForecastViewServiceShouldGetWeatherForecastsAsync(int noOfRecords, int expectedCount)
        {
            // define
            var dataBrokerMock = new Mock<IWeatherForecastDataBroker>();
            var notificationService = new WeatherForecastNotificationService();
            WeatherForecastListService? weatherForecastsViewService = new WeatherForecastListService(weatherForecastDataBroker: dataBrokerMock.Object, weatherForecastNotificationService: notificationService);

            dataBrokerMock.Setup(item =>
                item.GetWeatherForecastsAsync(Guid.NewGuid(), new ListOptions() ))
               .Returns(this.GetWeatherForecastListAsync(noOfRecords));

            // test
            await weatherForecastsViewService.GetForecastsAsync(new ListOptions());

            // assert
            Assert.IsType<List<DvoWeatherForecast>?>(weatherForecastsViewService.Records);
            Assert.Equal(expectedCount, weatherForecastsViewService.Records!.Count);
            dataBrokerMock.Verify(item => item.GetWeatherForecastsAsync(Guid.NewGuid(), new ListOptions()), Times.Once);
            dataBrokerMock.VerifyNoOtherCalls();
        }
    }
}
