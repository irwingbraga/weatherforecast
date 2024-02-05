using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.RequestHandlers.UpsertForecast;

public class UpsertForecastRequestHandler : IRequestHandler<UpsertForecastRequest, Guid>
{
    private readonly IForecastRepository _repository;

    public UpsertForecastRequestHandler(IForecastRepository repository) =>
        _repository = repository;

    public async Task<Guid> Handle(UpsertForecastRequest request, CancellationToken cancellationToken)
    {
        var domainEntity = await _repository.GetByDateAsync(request.Date, cancellationToken);
        if (domainEntity is null)
        {
            domainEntity = new Forecast(request.Date, request.Temperature);
            await _repository.AddAsync(domainEntity, cancellationToken);
            return domainEntity.Id;
        }

        domainEntity.UpdateTemperature(request.Temperature);
        await _repository.UpdateAsync(domainEntity, cancellationToken);

        return domainEntity.Id;
    }
}