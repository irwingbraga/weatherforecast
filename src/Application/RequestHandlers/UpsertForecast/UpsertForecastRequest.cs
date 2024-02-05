using MediatR;

namespace Application.RequestHandlers.UpsertForecast;

public record UpsertForecastRequest(DateTime Date, int Temperature) : IRequest<Guid>;