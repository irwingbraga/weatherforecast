using MediatR;

namespace Application.RequestHandlers.GetForecasts;

public record GetForecastsRequest(DateTime StartDate) : IRequest<IEnumerable<ForecastResponse>?>;
