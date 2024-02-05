using Domain.Entities;

namespace Domain.Repositories;

public interface IForecastRepository
{
    Task<Forecast?> GetByDateAsync(DateTime dateTime, CancellationToken cancellationToken);
    Task<IEnumerable<Forecast>> GetByRangeDateAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken);
    Task AddAsync(Forecast forecast, CancellationToken cancellationToken);
    Task UpdateAsync(Forecast forecast, CancellationToken cancellationToken);
}
