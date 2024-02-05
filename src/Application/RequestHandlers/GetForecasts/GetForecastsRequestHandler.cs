using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.RequestHandlers.GetForecasts;

public class GetForecastsRequestHandler : IRequestHandler<GetForecastsRequest, IEnumerable<ForecastResponse>?>
{
    private readonly IForecastRepository _forecastRepository;
    private readonly ITemperatureRangeRepository _temperatureRangeRepository;

    public GetForecastsRequestHandler(
        IForecastRepository forecastRepository,
        ITemperatureRangeRepository temperatureRangeRepository)
    {
        _forecastRepository = forecastRepository;
        _temperatureRangeRepository = temperatureRangeRepository;
    }

    public async Task<IEnumerable<ForecastResponse>?> Handle(GetForecastsRequest request, CancellationToken cancellationToken)
    {
        var endDate = request.StartDate.AddDays(7);
        var domainForecasts = await _forecastRepository.GetByRangeDateAsync(request.StartDate, endDate, cancellationToken);
        if (!domainForecasts.Any())
        {
            return default;
        }

        return await GetDescriptionForTemperaturesAsync(domainForecasts, cancellationToken);
    }

    public async Task<IEnumerable<ForecastResponse>> GetDescriptionForTemperaturesAsync(IEnumerable<Forecast> forecasts, CancellationToken cancellationToken)
    {
        var temperatureRanges = await _temperatureRangeRepository.GetAllAsync(cancellationToken);

        var forecastsResponse = new List<ForecastResponse>();

        foreach (var forecast in forecasts)
        {
            var response = new ForecastResponse
            (
                Date: forecast.Date,
                Description: temperatureRanges?.FirstOrDefault(p => p.MaxTemperature.Value >= forecast.Temperature.Value
                && p.MinTemperature.Value <= forecast.Temperature.Value)?.Description ?? "Undefined"
            );

            forecastsResponse.Add(response);
        }

        return forecastsResponse;
    }
}