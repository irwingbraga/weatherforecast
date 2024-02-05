using Application.RequestHandlers.GetForecasts;
using Application.RequestHandlers.UpsertForecast;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastsController : ControllerBase
{
    private readonly ISender _sender;
    public WeatherForecastsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("weekly", Name = "GetWeatherForecast")]
    public Task<IEnumerable<ForecastResponse>?> GetAsync([FromQuery] DateTime startDate, CancellationToken cancellationToken)
        => _sender.Send(new GetForecastsRequest(startDate), cancellationToken);

    [HttpPost(Name = "UpsertWeatherForecast")]
    public async Task<IActionResult> UpsertAsync([FromBody] UpsertForecastRequest request, CancellationToken cancellationToken)
    {
        await _sender.Send(request, cancellationToken);
        return Created();
    }
}
