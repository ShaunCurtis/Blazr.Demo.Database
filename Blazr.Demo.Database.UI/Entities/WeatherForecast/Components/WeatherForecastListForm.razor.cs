﻿namespace Blazr.Demo.Database.UI;

public partial class WeatherForecastListForm : ComponentBase
{
    [Inject] private WeatherForecastsViewService? listViewService { get; set; }

    [Inject] private WeatherForecastViewService? recordViewService { get; set; }

    [Inject] private NavigationManager? navigationManager { get; set; }

    private bool isLoading => listViewService!.Records is null;

    public bool IsModal { get; set; }

    private IModalDialog? modalDialog { get; set; }

    private ComponentState loadState => isLoading ? ComponentState.Loading : ComponentState.Loaded;

    protected async override Task OnInitializedAsync()
    {
        await this.listViewService!.GetForecastsAsync();
        this.listViewService.ListChanged += this.OnListChanged;
    }

    private async Task DeleteRecord(Guid Id)
        => await this.recordViewService!.DeleteRecordAsync(Id);

    private async Task EditRecord(Guid Id)
    {
        if (this.IsModal)
        {
            var options = new ModalOptions();
            options.Set("Id",Id);
            await this.modalDialog!.ShowAsync<WeatherForecastEditForm>(options);
        }
        else
            this.navigationManager!.NavigateTo($"/WeatherForecast/Edit/{Id}");
    }

    private async Task ViewRecord(Guid Id)
    {
        if (this.IsModal)
        {
            var options = new ModalOptions();
            options.Set("Id", Id);
            await this.modalDialog!.ShowAsync<WeatherForecastViewForm>(options);
        }
        else
            this.navigationManager!.NavigateTo($"/WeatherForecast/View/{Id}");
    }

    private async Task AddRecordAsync()
        => await this.recordViewService!.AddRecordAsync(
            new DcoWeatherForecast
            {
                Date = DateTime.Now,
                Id = Guid.NewGuid(),
                Summary = "Balmy",
                TemperatureC = 14
            });

    private void OnListChanged(object? sender, EventArgs e)
        => this.InvokeAsync(this.StateHasChanged);

    public void Dispose()
        => this.listViewService!.ListChanged -= this.OnListChanged;
}

